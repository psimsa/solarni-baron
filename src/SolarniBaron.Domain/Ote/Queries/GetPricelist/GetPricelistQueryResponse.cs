namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponse(GetPricelistQueryResponseItem[] HourlyRateBreakdown, decimal ExchangeRate);
