using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQuery(DateOnly Date) : IQuery<GetPricelistQuery, GetPricelistQueryResponse>;
