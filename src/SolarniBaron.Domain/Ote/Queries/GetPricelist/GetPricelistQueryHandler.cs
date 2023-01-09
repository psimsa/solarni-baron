using AngleSharp;
using AngleSharp.Html.Dom;

using Microsoft.Extensions.Caching.Distributed;

using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Extensions;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public class GetPricelistQueryHandler : IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>
{
    private readonly IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> _getExchangeRateQueryHandler;
    private readonly IApiHttpClient _client;
    private readonly IDistributedCache _cache;

    public GetPricelistQueryHandler(
        IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> getExchangeRateQueryHandler,
        IApiHttpClient client, IDistributedCache cache)
    {
        _getExchangeRateQueryHandler = getExchangeRateQueryHandler;
        _client = client;
        _cache = cache;
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

                var table = document.QuerySelectorAll("div.bigtable table.report_table")[1];

                var rows = table.QuerySelectorAll("tr");

                var dataRows = rows.Skip(1).OfType<IHtmlTableRowElement>().ToList();
                var hour = 1;

                var vatPct = 21;
                var surcharge = 300;

                var data = dataRows.Take(dataRows.Count() - 1).Select(row =>
                {
                    GetPricelistQueryResponseItem toReturn = GetPricelistQueryResponseItem.Empty;
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
                    new GetPricelistQueryResponse(ResponseStatus.Ok, data.ToArray(), exchangeRate);
                return getPricelistQueryResponse;
            });
        return queryResponse ?? GetPricelistQueryResponse.Empty();
    }
}
