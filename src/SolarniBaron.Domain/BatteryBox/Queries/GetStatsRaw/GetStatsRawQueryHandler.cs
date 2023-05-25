using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using DotnetDispatcher.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Extensions;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStatsRaw;

public class GetStatsRawQueryHandler : IQueryHandler<GetStatsRawQuery, GetStatsRawQueryResponse>
{
    private readonly IBatteryBoxDataConnector _dataConnector;
    private readonly ICache _cache;
    private readonly ILogger<GetStatsRawQueryHandler> _logger;

    public GetStatsRawQueryHandler(IBatteryBoxDataConnector dataConnector, ICache cache,
        ILogger<GetStatsRawQueryHandler> logger)
    {
        _dataConnector = dataConnector;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetStatsRawQueryResponse> Query(GetStatsRawQuery getStatsQuery, CancellationToken cancellationToken = default)
    {
        (string username, string password, string? unitId) = getStatsQuery;

        var cacheKeyBytes = SHA1.HashData(Encoding.UTF8.GetBytes($"bbud-{username}-{password}-{unitId ?? string.Empty}"));
        var cacheKey = Convert.ToBase64String(cacheKeyBytes);

        var cachedItem = await _cache.GetAsync(cacheKey);
        if (cachedItem is not null)
        {
            _logger.LogCacheHit();
            return new GetStatsRawQueryResponse(JsonSerializer.Deserialize<Dictionary<string, JsonNode>>(cachedItem)!);
        }

        _logger.LogCacheNotHit();
        try
        {
            var rawData = await _dataConnector.GetRawStats(username, password);
            var stats = JsonSerializer.Deserialize<Dictionary<string, JsonNode>>(rawData);

            var dateTimeString = stats?.FirstOrDefault().Value["device"]?["lastcall"]?.GetValue<string>();
            var pragueTimeZoneInfo = DateTimeHelpers.GetPragueTimeZoneInfo();

            var parsedDate = DateTime.ParseExact(dateTimeString,
                "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);

            TimeSpan pragueTimeZoneOffset = pragueTimeZoneInfo.GetUtcOffset(parsedDate);

            var dateTimeOffset= new DateTimeOffset(parsedDate, pragueTimeZoneOffset);

            var nextRefresh = dateTimeOffset.AddSeconds(110) - DateTime.Now;
            nextRefresh = nextRefresh.TotalSeconds > 9 ? nextRefresh : TimeSpan.FromSeconds(9);

            var absoluteExpiration = DateTimeOffset.UtcNow.Add(nextRefresh);

            await _cache.SetAsync(cacheKey, JsonSerializer.SerializeToUtf8Bytes(stats),
                new DistributedCacheEntryOptions() {AbsoluteExpiration = absoluteExpiration});

            return new GetStatsRawQueryResponse(stats);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting stats");
            throw;
        }
    }
}
