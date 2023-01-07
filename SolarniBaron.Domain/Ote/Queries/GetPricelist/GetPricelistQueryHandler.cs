using AngleSharp;
using AngleSharp.Html.Dom;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public class GetPricelistQueryHandler : IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>
{
    private readonly IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> _getExchangeRateQueryHandler;
    private readonly HttpClient _client;

    public GetPricelistQueryHandler(
        IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse> getExchangeRateQueryHandler,
        HttpClient client)
    {
        _getExchangeRateQueryHandler = getExchangeRateQueryHandler;
        _client = client;
    }

    public async Task<GetPricelistQueryResponse> Get(IQuery<GetPricelistQuery, GetPricelistQueryResponse> query)
    {
        var exchangeRate = await _getExchangeRateQueryHandler.Get(new GetExchangeRateQuery(query.Query!.Date));

        var baseUrl = "https://www.ote-cr.cz/cs/kratkodobe-trhy/elektrina/denni-trh";
        var sourceDate = DateTime.Parse("2022-01-30");
        var date = sourceDate.ToString("yyyy-MM-dd");
        var url = $"{baseUrl}/?date={date}";
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
                var price = decimalData * exchangeRate.Rate;
                var withSurcharge = price + surcharge;
                var vat = withSurcharge * vatPct / 100;
                var total = withSurcharge + vat;

                toReturn = new GetPricelistQueryResponseItem(hour, decimalData, price, withSurcharge,
                    vat, total);
            }

            hour++;
            return toReturn;
        });
        
        return new GetPricelistQueryResponse(ResponseStatus.Ok, data.ToArray(), exchangeRate.Rate);
    }
}
