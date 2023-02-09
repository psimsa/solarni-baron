using SolarniBaron.Domain.Ote.Models;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

namespace SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

public record GetPriceOutlookQueryResponse(IReadOnlyCollection<PriceListItem>? HourlyRateBreakdown);
