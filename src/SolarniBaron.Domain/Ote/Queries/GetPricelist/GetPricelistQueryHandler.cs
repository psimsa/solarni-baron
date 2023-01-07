using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using AngleSharp;
using AngleSharp.Html.Dom;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Extensions;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public partial class GetPricelistQueryHandler : IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>
{
    private readonly IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> _getExchangeRateQueryHandler;
    private readonly IApiHttpClient _client;
    private readonly ICache _cache;
    private readonly ILogger<GetPricelistQueryHandler> _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data: {Error}")]
    private partial void LogErrorGettingOteData(string error);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Cache not hit when getting OTE data for {date} with key {key}")]
    private partial void LogCacheNotHit(string date, string key);

    public GetPricelistQueryHandler(
        IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> getExchangeRateQueryHandler,
        IApiHttpClient client, ICache cache, ILogger<GetPricelistQueryHandler> logger)
    {
        _getExchangeRateQueryHandler = getExchangeRateQueryHandler;
        _client = client;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetPricelistQueryResponse> Get(IQuery<GetPricelistQuery, GetPricelistQueryResponse> query)
    {
        var getPricelistQuery = query.Data ?? throw new ArgumentException("Invalid query type");

        var cacheKeyBytes = SHA1.HashData(Encoding.UTF8.GetBytes($"prices-{getPricelistQuery.Date:yyyy-MM-dd}"));
        var cacheKey = Convert.ToBase64String(cacheKeyBytes);

        var exchangeRateQuery = new GetExchangeRateQuery(getPricelistQuery.Date);
        var exchangeRateQueryResponse = await _getExchangeRateQueryHandler.Get(exchangeRateQuery);

        var exchangeRate = exchangeRateQueryResponse.Rate;

        var vatPct = 21;
        var surcharge = 300;

        var priceString = await _cache.GetOrCreateAsync(cacheKey,
            async () =>
            {
                LogCacheNotHit(getPricelistQuery.Date.ToString("yyyy-MM-dd"), cacheKey);

                var date = getPricelistQuery.Date.ToString("yyyy-MM-dd");
                var url = $"{Constants.OteUrl}/?date={date}";
                var content = await _client.GetStringAsync(url);
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(req => req.Content(content));
                try
                {
                    var reportTables = document.QuerySelectorAll("div.bigtable table.report_table");
                    if (reportTables.Length < 2)
                    {
                        LogErrorGettingOteData("No data found");
                        return null;
                    }

                    var table = reportTables[1];

                    var rows = table.QuerySelectorAll("tr");

                    var dataRows = rows.Skip(1).OfType<IHtmlTableRowElement>().ToList();


                    var data = dataRows.Take(dataRows.Count() - 1).Select(row =>
                    {
                        var toReturn = GetPricelistQueryResponseItem.Empty;
                        var data = row.Cells[1].TextContent;
                        var isValid = decimal.TryParse(data.Replace(',', '.'), out var basePriceEur);
                        return basePriceEur.ToString(CultureInfo.InvariantCulture);
                    });
                    return string.Join('|', data);
                }
                catch (Exception e)
                {
                    LogErrorGettingOteData(e.Message);
                    return null;
                }
            }, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2) });
        if (priceString == null)
            return new GetPricelistQueryResponse(null, ResponseStatus.Error, "Error getting pricelist");

        var basicPrices = priceString.Split('|').Select(item =>
        {
            var isValid = decimal.TryParse(item, out var basePriceEur);
            return isValid ? basePriceEur : 0;
        }).Select((item, index) => new KeyValuePair<int, decimal>(index, item)).OrderBy(_ => _.Value).ToList();

        var toReturn = new GetPricelistQueryResponseItem[24];
        for (int i = 0; i < 24; i++)
        {
            var item = basicPrices[i];
            decimal basePriceCzk = item.Value * exchangeRate;
            decimal basePriceCzkVat = basePriceCzk * vatPct / 100;
            decimal basePriceCzkTotal = basePriceCzk + basePriceCzkVat;
            decimal withSurchargeCzk = basePriceCzk + surcharge;
            decimal withSurchargeCzkVat = withSurchargeCzk * vatPct / 100;
            decimal withSurchargeCzkTotal = withSurchargeCzk + withSurchargeCzkVat;

            var toReturnItem = new GetPricelistQueryResponseItem(item.Key + 1,
                item.Value,
                basePriceCzk,
                basePriceCzkVat,
                basePriceCzkTotal,
                withSurchargeCzk,
                withSurchargeCzkVat,
                withSurchargeCzkTotal,
                i);
            toReturn[item.Key] = toReturnItem;
        }

        return new GetPricelistQueryResponse(new GetPricelistQueryResponseData(toReturn, exchangeRate));
    }
}
