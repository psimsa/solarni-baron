using SolarniBaron.Domain.Ote.Models;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponse(PriceListItem[]? HourlyRateBreakdown, decimal ExchangeRate);
