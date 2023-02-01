using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Extensions;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public class GetStatsQueryHandler : IQueryHandler<GetStatsQuery, GetStatsQueryResponse>
{
    private readonly IBatteryBoxDataConnector _dataConnector;
    private readonly IDistributedCache _cache;
    private readonly ILogger<GetStatsQueryHandler> _logger;

    public GetStatsQueryHandler(IBatteryBoxDataConnector dataConnector, IDistributedCache cache,
        ILogger<GetStatsQueryHandler> logger)
    {
        _dataConnector = dataConnector;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetStatsQueryResponse> Get(IQuery<GetStatsQuery, GetStatsQueryResponse> query)
    {
        var getStatsQuery = query.Data ?? throw new ArgumentException("Invalid query type");

        (string username, string password, string? unitId) = getStatsQuery;

        var cacheKeyBytes = SHA1.HashData(Encoding.UTF8.GetBytes($"bbstats-{username}-{password}-{unitId ?? string.Empty}"));
        var cacheKey = Convert.ToBase64String(cacheKeyBytes);

        var cachedItem = await _cache.GetAsync(cacheKey);
        if (cachedItem is not null)
        {
            _logger.LogCacheHit();
            return new GetStatsQueryResponse(JsonSerializer.Deserialize<BatteryBoxStatus>(cachedItem)!);
        }

        _logger.LogCacheNotHit();
        try
        {
            var fromApi = await _dataConnector.GetStatsForUnit(username, password, unitId);
            var stats = BatteryBoxStatus.FromBatteryBoxUnitData(fromApi, unitId);

            var nextRefresh = stats.LastCall.AddSeconds(110) - DateTime.Now;
            nextRefresh = nextRefresh.TotalSeconds > 9 ? nextRefresh : TimeSpan.FromSeconds(9);

            var absoluteExpiration = DateTimeOffset.UtcNow.Add(nextRefresh);

            await _cache.SetAsync(cacheKey, JsonSerializer.SerializeToUtf8Bytes(stats),
                new DistributedCacheEntryOptions() { AbsoluteExpiration = absoluteExpiration });

            _logger.LogDateInfo(stats.LastCall, nextRefresh, absoluteExpiration);

            return new GetStatsQueryResponse(stats);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting stats");
            return new GetStatsQueryResponse(BatteryBoxStatus.Empty(), ResponseStatus.Error, e.Message);
        }
    }
}
