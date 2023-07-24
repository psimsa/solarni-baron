using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using DotnetDispatcher.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.Clustering;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Models;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public partial class GetPricelistQueryHandler : IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>
{
    private readonly ISolarniBaronDispatcher _dispatcher;
    private readonly IPriceClusteringWorker _priceClusteringWorker;
    private readonly IOteService _oteService;
    private readonly ICache _cache;
    private readonly ILogger<GetPricelistQueryHandler> _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data: {Error}")]
    private partial void LogErrorGettingOteData(string error);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Cache not hit when getting OTE data for {date} with key {key}")]
    private partial void LogCacheNotHit(string date, string key);

    public GetPricelistQueryHandler(
        ISolarniBaronDispatcher dispatcher,
        IPriceClusteringWorker priceClusteringWorker,
        IOteService oteService, ICache cache, ILogger<GetPricelistQueryHandler> logger)
    {
        _dispatcher = dispatcher;
        _priceClusteringWorker = priceClusteringWorker;
        _oteService = oteService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetPricelistQueryResponse> Query(GetPricelistQuery getPricelistQuery, CancellationToken cancellationToken)
    {
        var cacheKeyBytes = SHA1.HashData(Encoding.UTF8.GetBytes($"prices-{getPricelistQuery.Date:yyyy-MM-dd}"));
        var cacheKey = Convert.ToBase64String(cacheKeyBytes);

        var exchangeRateQuery = new GetExchangeRateQuery(getPricelistQuery.Date);
        var exchangeRateQueryResponse = await _dispatcher.Dispatch(exchangeRateQuery, cancellationToken);

        var exchangeRate = exchangeRateQueryResponse.Data.Rate;

        var vatPct = 21;
        var surcharge = 300;

        var priceString = await _cache.GetOrCreateAsync(cacheKey,
            async () =>
            {
                LogCacheNotHit(getPricelistQuery.Date.ToString("yyyy-MM-dd"), cacheKey);

                /*var ote = new PublicDataServiceSoapClient();

                var result = await ote.GetDamPriceEAsync(getPricelistQuery.Date.ToDateTime(TimeOnly.MinValue), getPricelistQuery.Date.ToDateTime(TimeOnly.MaxValue), 0, 24, true);

                var priceItems = result.Result.Select(x => x.Price.ToString(CultureInfo.InvariantCulture));*/
                var priceItems = await _oteService.GetPricesForDay(getPricelistQuery.Date.ToDateTime(TimeOnly.MinValue));

                var priceList = string.Join('|', priceItems.Select(x=>x.ToString(CultureInfo.InvariantCulture)));
                return priceList;
            }, new DistributedCacheEntryOptions() {AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)});

        switch (priceString)
        {
            case null:
                // TODO: return error
                await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes("error"),
                    new DistributedCacheEntryOptions() {AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)}, cancellationToken);
                return new GetPricelistQueryResponse(null, 0);
            case "error":
                return new GetPricelistQueryResponse(null, 0);
        }

        var basicPrices = priceString.Split('|').Select(item =>
        {
            var isValid = decimal.TryParse(item, CultureInfo.InvariantCulture, out var basePriceEur);
            return isValid ? basePriceEur : 0;
        }).Select((item, index) => new KeyValuePair<int, decimal>(index, item)).OrderBy(_ => _.Value).ToList();

        var clusters = _priceClusteringWorker.GetClusters(basicPrices.Select(_ => _.Value).ToArray(), 4);

        var toReturn = new PriceListItem[24];
        for (int i = 0; i < 24; i++)
        {
            var item = basicPrices[i];
            decimal basePriceCzk = item.Value * exchangeRate;
            decimal basePriceCzkVat = basePriceCzk * vatPct / 100;
            decimal basePriceCzkTotal = basePriceCzk + basePriceCzkVat;
            decimal withSurchargeCzk = basePriceCzk + surcharge;
            decimal withSurchargeCzkVat = withSurchargeCzk * vatPct / 100;
            decimal withSurchargeCzkTotal = withSurchargeCzk + withSurchargeCzkVat;
            decimal buyPriceCzk = basePriceCzk - 600;

            var toReturnItem = new PriceListItem(item.Key,
                item.Value,
                basePriceCzk,
                basePriceCzkVat,
                basePriceCzkTotal,
                withSurchargeCzk,
                withSurchargeCzkVat,
                withSurchargeCzkTotal,
                buyPriceCzk,
                i,
                clusters[i]);
            toReturn[item.Key] = toReturnItem;
        }

        return new GetPricelistQueryResponse(toReturn, exchangeRate);
    }
}
