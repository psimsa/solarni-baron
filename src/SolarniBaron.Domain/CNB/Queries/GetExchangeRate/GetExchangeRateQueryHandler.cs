using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public class GetExchangeRateQueryHandler : IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse>
{
    private readonly HttpClient _client;

    public GetExchangeRateQueryHandler(HttpClient client)
    {
        _client = client;
    }

    public async Task<GetExchangeRateQueryResponse> Get(
        IQuery<GetExchangeRateQuery, GetExchangeRateQueryResponse> query)
    {
        var response = await _client.GetStringAsync(
            $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={query.Query.Date.ToString("dd.MM.yyyy")}");
        var euroLine = response.Split('\n').FirstOrDefault(line => line.Contains("EMU"));
        var euroRate = euroLine?.Split('|')[4];
        var success = decimal.TryParse(euroRate?.Replace(',', '.'), out var rateValue);
        return new GetExchangeRateQueryResponse(success ? rateValue : 0);
    }
}
