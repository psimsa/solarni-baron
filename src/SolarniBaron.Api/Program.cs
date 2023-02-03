using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using SolarniBaron.Caching;
using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Commands;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Persistence;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDomain();
builder.Services.AddPersistence();

builder.Services.AddHttpClient();

builder.Services.RegisterCache(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddCors();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(cp =>
{
    cp.AllowAnyHeader();
    cp.AllowAnyMethod();
    cp.WithOrigins(app.Configuration["AllowedOrigins"]?.Split(",") ?? Array.Empty<string>());
});

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

/* app.MapGet("api/protected", () => Results.Ok("hello world"))
    .RequireAuthorization(); */

app.MapGet("/healthz", () => Results.Text("OK"));

app.MapGet("api/ote/day/{date?}",
        async (DateOnly? date, IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> queryHandler) =>
        {
            var result = await queryHandler.Get(
                new GetPricelistQuery(date ?? DateOnly.FromDateTime(DateTimeHelpers.GetPragueDateTimeNow())));
            return result.ResponseStatus == ResponseStatus.Error ? Results.NotFound() : Results.Ok(result);
        })
    .WithName("GetPricelist")
    .Produces<GetPricelistQueryResponse>()
    .WithOpenApi();

app.MapGet("api/ote/now",
        async (IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> queryHandler) =>
        {
            var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();
            var result = await queryHandler.Get(new GetPricelistQuery(DateOnly.FromDateTime(pragueDateTimeNow)));
            if(result.ResponseStatus == ResponseStatus.Error)
            {
                return Results.BadRequest(result);
            }

            var toReturn = result.Data?.HourlyRateBreakdown.FirstOrDefault(x => x.HourIndex + 1 == pragueDateTimeNow.Hour);
            return toReturn == null ? Results.NotFound() : Results.Ok(toReturn);
        })
    .WithName("GetCurrentPrice")
    .Produces<ApiResponse<GetPricelistQueryResponseItem>>()
    .WithOpenApi();

app.MapPost("api/batterybox/getstats",
        async ([FromBody] LoginInfo loginInfo, IQueryHandler<GetStatsQuery, GetStatsQueryResponse> queryHandler, ILogger<Program> logger) =>
        {
            var data = await queryHandler.Get(new GetStatsQuery(loginInfo.Email, loginInfo.Password, loginInfo.UnitId));
            if (data.ResponseStatus == ResponseStatus.Error)
            {
                logger.LogError("Error fetching data: {error}", data.Error);
                return Results.BadRequest(data.Error);
            }

            return Results.Ok(data.BatteryBoxStatus);
        })
    .WithName("GetStats")
    .Produces<BatteryBoxStatus>()
    .WithOpenApi();

app.MapPost("api/batterybox/setmode",
        async ([FromBody] SetModeInfo setModeInfo, ICommandHandler<SetModeCommand, SetModeCommandResponse> commandHandler,
            ILogger<Program> logger) =>
        {
            var data = await commandHandler.Execute(new SetModeCommand(setModeInfo.Email, setModeInfo.Password, setModeInfo.UnitId,
                setModeInfo.Mode));
            if (data.ResponseStatus == ResponseStatus.Error)
            {
                logger.LogError("Error setting mode to {mode}: {error}", setModeInfo.Mode, data.Error);
                return Results.BadRequest(data.Error);
            }

            return Results.Ok(data);
        })
    .WithName("SetMode")
    .Produces<SetModeCommandResponse>()
    .WithOpenApi();

app.Run();

namespace SolarniBaron.Api
{
    public partial class Program
    {
    }
}
