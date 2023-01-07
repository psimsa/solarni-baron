using SolarniBaron.Domain.Contracts.Queries;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQuery(DateOnly Date) : IQuery<GetPricelistQuery, GetPricelistQueryResponse>;
