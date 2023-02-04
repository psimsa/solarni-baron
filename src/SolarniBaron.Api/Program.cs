using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using SolarniBaron.Caching;
using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;
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

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddCors();

// builder.Services.AddAuthorization();


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

/*app.UseAuthentication();
app.UseAuthorization();*/

// app.MapControllers();

/* app.MapGet("api/protected", () => Results.Ok("hello world"))
    .RequireAuthorization(); */

app.MapGet("/healthz", () => Results.Text("OK"));

/*app.MapGet("/test", async (ISolarniBaronDispatcher dispatcher) =>
{
    var response = await dispatcher.Dispatch(new GetPriceOutlookQuery());
    return Results.Ok(response);
}).WithOpenApi();*/

app.MapGet("api/ote/day/{date?}",
        async (DateOnly? date, ISolarniBaronDispatcher dispatcher) =>
        {
            var result = await dispatcher.Dispatch(
                new GetPricelistQuery(date ?? DateOnly.FromDateTime(DateTimeHelpers.GetPragueDateTimeNow())));
            return result == null ? Results.NotFound() : Results.Ok(result);
        })
    .WithName("GetPricelist")
    .Produces<GetPricelistQueryResponse>()
    .WithOpenApi();

app.MapGet("api/ote/now",
        async (ISolarniBaronDispatcher dispatcher) =>
        {
            var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();
            var result = await dispatcher.Dispatch(new GetPricelistQuery(DateOnly.FromDateTime(pragueDateTimeNow)));
            if (result is null)
            {
                return Results.BadRequest();
            }

            var toReturn = result.HourlyRateBreakdown.FirstOrDefault(x => x.HourIndex == pragueDateTimeNow.Hour);
            return toReturn == null ? Results.NotFound() : Results.Ok((toReturn));
        })
    .WithName("GetCurrentPrice")
    .Produces<PriceListItem>()
    .ProducesProblem(400)
    .ProducesProblem(404)
    .WithOpenApi();

app.MapGet("api/ote/outlook",
        async (ISolarniBaronDispatcher dispatcher) =>
        {
            var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();

            var result = await dispatcher.Dispatch(new GetPriceOutlookQuery(pragueDateTimeNow));
            if (result is null || result.Status != ResponseStatus.Ok)
            {
                return Results.BadRequest(result?.ErrorMessage);
            }

            return Results.Ok(result.Data);
        })
    .WithName("GetPriceOutlook")
    .Produces<GetPriceOutlookQueryResponse>()
    .ProducesProblem(400)
    .WithOpenApi();

app.MapGet("api/ote/outlook/now",
        async (ISolarniBaronDispatcher dispatcher) =>
        {
            var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();

            var result = await dispatcher.Dispatch(new GetPriceOutlookQuery(pragueDateTimeNow));
            if (result is null || result.Status != ResponseStatus.Ok)
            {
                return Results.BadRequest(result?.ErrorMessage);
            }

            return Results.Ok(result.Data.HourlyRateBreakdown.FirstOrDefault(x => x.HourIndex == pragueDateTimeNow.Hour));
        })
    .WithName("GetPriceOutlook")
    .Produces<GetPriceOutlookQueryResponse>()
    .ProducesProblem(400)
    .WithOpenApi();

app.MapPost("api/batterybox/getstats",
        async ([FromBody] LoginInfo loginInfo, ISolarniBaronDispatcher dispatcher, ILogger<Program> logger) =>
        {
            try
            {
                var data = await dispatcher.Dispatch(new GetStatsQuery(loginInfo.Email, loginInfo.Password, loginInfo.UnitId));
                if (data is null)
                {
                    logger.LogError("Error fetching data");
                    return Results.BadRequest();
                }

                return Results.Ok(data.BatteryBoxStatus);
            }
            catch (Exception e)
            {
                logger.LogError("Error fetching data: {error}", e.Message);
                return Results.BadRequest(e.Message);
            }
        })
    .WithName("GetStats")
    .Produces<BatteryBoxStatus>()
    .WithOpenApi();

app.MapPost("api/batterybox/setmode",
        async ([FromBody] SetModeInfo setModeInfo, ISolarniBaronDispatcher dispatcher,
            ILogger<Program> logger) =>
        {
            var data = await dispatcher.Dispatch(new SetModeCommand(setModeInfo.Email, setModeInfo.Password, setModeInfo.UnitId,
                setModeInfo.Mode));
            if (!string.IsNullOrWhiteSpace(data.Error))
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
