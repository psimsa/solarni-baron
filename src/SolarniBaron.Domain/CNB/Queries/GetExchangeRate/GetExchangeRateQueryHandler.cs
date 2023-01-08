using Microsoft.Extensions.Caching.Distributed;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Extensions;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public class GetExchangeRateQueryHandler : IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse>
{
    private readonly IApiHttpClient _client;
    private readonly IDistributedCache _cache;

    public GetExchangeRateQueryHandler(IApiHttpClient client, IDistributedCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<GetExchangeRateQueryResponse> Get(
        IQuery<GetExchangeRateQuery, GetExchangeRateQueryResponse> query)
    {
        var getExchangeRateQuery = query.Data ?? throw new ArgumentException("Invalid query type");
        return await _cache.GetOrCreateAsync($"pricelist-{getExchangeRateQuery.Date:yyyy-MM-dd}", async () =>
        {
            var response = await _client.GetStringAsync(
                $"{Constants.CnbUrl}?date={getExchangeRateQuery.Date.ToString("dd.MM.yyyy")}");
            var euroLine = response.Split('\n').FirstOrDefault(line => line.StartsWith("EMU"));
            var euroRate = euroLine?.Split('|')[4];
            var success = decimal.TryParse(euroRate?.Replace(',', '.'), out var rateValue);
            return new GetExchangeRateQueryResponse(success ? rateValue : 0);
        }) ?? GetExchangeRateQueryResponse.Empty();
    }
}
