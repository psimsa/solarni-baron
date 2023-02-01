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
    private readonly IDistributedCache _cache;
    private readonly ILogger<GetPricelistQueryHandler> _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data: {Error}")] private partial void LogErrorGettingOteData(string error);

    public GetPricelistQueryHandler(
        IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> getExchangeRateQueryHandler,
        IApiHttpClient client, IDistributedCache cache, ILogger<GetPricelistQueryHandler> logger)
    {
        _getExchangeRateQueryHandler = getExchangeRateQueryHandler;
        _client = client;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetPricelistQueryResponse> Get(IQuery<GetPricelistQuery, GetPricelistQueryResponse> query)
    {
        var getPricelistQuery = query.Data ?? throw new ArgumentException("Invalid query type");
        var queryResponse = await _cache.GetOrCreateAsync($"pricelist-{getPricelistQuery.Date:yyyy-MM-dd}",
            async () =>
            {
                var exchangeRateQuery = new GetExchangeRateQuery(getPricelistQuery.Date);
                var exchangeRateQueryResponse = await _getExchangeRateQueryHandler.Get(exchangeRateQuery);

                var exchangeRate = exchangeRateQueryResponse.Rate;

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
                    var hour = 1;

                    var vatPct = 21;
                    var surcharge = 300;

                    var data = dataRows.Take(dataRows.Count() - 1).Select(row =>
                    {
                        var toReturn = GetPricelistQueryResponseItem.Empty;
                        var data = row.Cells[1].TextContent;
                        var isValid = decimal.TryParse(data.Replace(',', '.'), out var decimalData);
                        if (isValid)
                        {
                            var price = decimalData * exchangeRate;
                            var withSurcharge = price + surcharge;
                            var vat = withSurcharge * vatPct / 100;
                            var total = withSurcharge + vat;

                            toReturn = new GetPricelistQueryResponseItem(hour, decimalData, price, withSurcharge,
                                vat, total);
                        }

                        hour++;
                        return toReturn;
                    });

                    var getPricelistQueryResponse =
                        new GetPricelistQueryResponse(new GetPricelistQueryResponseData(data.ToArray(), exchangeRate), ResponseStatus.Ok);
                    return getPricelistQueryResponse;
                }
                catch (Exception e)
                {
                    LogErrorGettingOteData(e.Message);
                    return null;
                }
            });
        return queryResponse ?? new GetPricelistQueryResponse(null, ResponseStatus.Error, "Error getting pricelist");
    }
}
