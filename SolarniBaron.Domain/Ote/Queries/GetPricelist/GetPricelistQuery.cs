using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQuery(DateOnly date) : IQuery<GetPricelistQueryResponse>;