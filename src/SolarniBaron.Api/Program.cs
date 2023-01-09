using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

using SolarniBaron.Api;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Persistence;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomain();
builder.Services.AddPersistence();

builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// app.MapControllers();

app.MapGet("api/protected", () => Results.Ok("hello world"))
    .RequireAuthorization();

app.MapGet("/healthz", () => Results.Text("OK"));

app.MapGet("api/ote/{date}",
        async (DateOnly date, IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> query) =>
        {
            var result = await query.Get(new GetPricelistQuery(date));
            if (result.Status == ResponseStatus.Error)
            {
                return Results.NotFound();
            }

            var response = new ApiResponse<GetPricelistQueryResponse>(result, result.Status, "");
            return Results.Ok(response);
        })
    .WithName("GetPricelist")
    .Produces<ApiResponse<GetPricelistQueryResponse>>()
    .WithOpenApi()
    ;

app.MapPost("api/batterybox/getstats",
        async ([FromBody] LoginInfo loginInfo, IQueryHandler<GetStatsQuery, GetStatsQueryResponse> queryHandler, ILogger<Program> logger) =>
        {
            var data = await queryHandler.Get(new GetStatsQuery(loginInfo.Email, loginInfo.Password, loginInfo.UnitId));
            if (data.ResponseStatus == ResponseStatus.Error)
            {
                logger.LogError(data.Error);
                return Results.BadRequest(data.Error);
            }

            return Results.Ok(data.BatteryBoxStatus);
        })
    .WithName("GetStats")
    .Produces<BatteryBoxStatus>()
    .WithOpenApi();

app.Run();

namespace SolarniBaron.Api
{
    public partial class Program { }
}
