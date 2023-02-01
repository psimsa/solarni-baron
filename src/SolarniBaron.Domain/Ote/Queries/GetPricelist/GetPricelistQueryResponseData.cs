namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponseData(GetPricelistQueryResponseItem[] HourlyRateBreakdown, decimal ExchangeRate);
