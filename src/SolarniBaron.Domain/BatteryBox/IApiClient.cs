using System.Text.Json;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.BatteryBox.Models.Fve;

namespace SolarniBaron.Domain.BatteryBox;

public interface IApiClient : IDisposable
{
    Task<string> GetRawStats(string username, string password);
    Task<(bool, string?)> SetMode(string username, string password, string unitId, string mode);
}

public interface IDataConnector
{
    Task<string> GetRawStats(string username, string password);

    Task<FveObject> GetStatsForUnit(string username, string password, string? unitId);

    Task<(bool, string?)> SetMode(string username, string password, string unitId, string mode);
}

public partial class OigDataConnector : IDisposable, IDataConnector
{
    private readonly ILogger<OigDataConnector> _logger;
    private readonly IApiClient _client;

    [LoggerMessage(1102, LogLevel.Debug, "Deserialized status response {statusResponse}")]
    public partial void LogDeserializedStatusResponse(FveObject statusResponse);

    [LoggerMessage(1103, LogLevel.Debug, "Converting response to FveStatus")]
    public partial void LogConvertingResponseToFveStatus();

    [LoggerMessage(1104, LogLevel.Debug, "Setting mode of {unitId} to {mode}")]
    public partial void LogSettingModeToMode(string unitId, string mode);


    [LoggerMessage(1204, LogLevel.Warning, "Unit {unitId} not found in response.")]
    public partial void LogUnitNotFound(string unitId);


    [LoggerMessage(1301, LogLevel.Error, "Error saving stats to Cosmos DB")]
    public partial void LogErrorSavingStatsToCosmosDb();

    [LoggerMessage(1302, LogLevel.Error, "Error deserializing response {responseString}")]
    public partial void LogErrorDeserializingResponse(string responseString);

    [LoggerMessage(1303, LogLevel.Error, "Deserialized stats is null")]
    public partial void LogDeserializedStatsIsNull();

    public OigDataConnector(IApiClient client, ILogger<OigDataConnector> logger)
    {
        _client = client;
        _logger = logger;
    }


    public async Task<string> GetRawStats(string username, string password)
    {
        return await _client.GetRawStats(username, password);
    }

    public async Task<FveObject> GetStatsForUnit(string username, string password, string? unitId)
    {
        var currentState = await _client.GetRawStats(username, password);
        try
        {
            var stateObject = JsonSerializer.Deserialize(currentState,
                CommonSerializationContext.Default.DictionaryStringFveObject);

            if (stateObject is null)
            {
                LogDeserializedStatsIsNull();
                throw new Exception();
            }

            if (!string.IsNullOrWhiteSpace(unitId) && !stateObject.ContainsKey(unitId))
            {
                LogUnitNotFound(unitId);
                throw new Exception($"Unit {unitId} not found");
            }

            var fveObject = string.IsNullOrWhiteSpace(unitId)
                ? stateObject?.First().Value
                : stateObject[unitId];

            LogDeserializedStatusResponse(fveObject);

            if (fveObject != null)
            {
                try
                {
                    // var result = await container.UpsertItemAsync(new CosmosEntity<FveObject>(o => o.Device.Lastcall.ToUnixTimeSeconds().ToString(), fveObject));
                }
                catch (Exception ex)
                {
                    LogErrorSavingStatsToCosmosDb();
                }
            }

            return fveObject;
        }
        catch (JsonException ex)
        {
            LogErrorDeserializingResponse(currentState);
            throw;
        }
    }

    public async Task<(bool, string?)> SetMode(string username, string password, string unitId, string mode)
    {
        LogSettingModeToMode(unitId, mode);
        return await _client.SetMode(username, password, unitId, mode);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}

