using DotnetDispatcher.Core;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public record GetExchangeRateQuery(DateOnly Date) : IQuery<GetExchangeRateQueryResponse>;
