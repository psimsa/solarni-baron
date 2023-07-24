using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using DotnetDispatcher.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public partial class GetExchangeRateQueryHandler : IQueryHandler<GetExchangeRateQuery, Result<GetExchangeRateQueryResponse>>
{
    private readonly IApiHttpClient _client;
    private readonly ICache _cache;
    private readonly ILogger<GetExchangeRateQueryHandler> _logger;

    [LoggerMessage(EventId = 1, Level = LogLevel.Information,
        Message = "Cache not hit when getting exchange rate data for {date} with key {key}")]
    private partial void LogCacheNotHit(string date, string key);


    public GetExchangeRateQueryHandler(IApiHttpClient client, ICache cache, ILogger<GetExchangeRateQueryHandler> logger)
    {
        _client = client;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<GetExchangeRateQueryResponse>> Query(GetExchangeRateQuery getExchangeRateQuery,
        CancellationToken cancellationToken = default)
    {
        var cacheKeyBytes = SHA1.HashData(Encoding.UTF8.GetBytes($"eurczkrate-{getExchangeRateQuery.Date:yyyy-MM-dd}"));
        var cacheKey = Convert.ToBase64String(cacheKeyBytes);

        var rateText = await _cache.GetOrCreateAsync(cacheKey, async () =>
        {
            LogCacheNotHit(getExchangeRateQuery.Date.ToString("yyyy-MM-dd"), cacheKey);
            var response = await _client.GetStringAsync(
                $"{Constants.CnbUrl}?date={getExchangeRateQuery.Date:dd.MM.yyyy}");
            var euroLine = response.Split('\n').FirstOrDefault(line => line.StartsWith("EMU"));
            var euroRate = euroLine?.Split('|')[4];
            var success = decimal.TryParse(euroRate?.Replace(',', '.'), CultureInfo.InvariantCulture, out var rateValue);
            return rateValue.ToString(CultureInfo.InvariantCulture);
        }, new DistributedCacheEntryOptions() {AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)});

        var rate = decimal.Parse(rateText, CultureInfo.InvariantCulture);
        return new Result<GetExchangeRateQueryResponse>(ResponseStatus.Ok, new GetExchangeRateQueryResponse(rate));
    }
}
