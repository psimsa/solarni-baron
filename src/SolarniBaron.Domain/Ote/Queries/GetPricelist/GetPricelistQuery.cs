using DotnetDispatcher.Core;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQuery(DateOnly Date) : IQuery<GetPricelistQueryResponse>;
