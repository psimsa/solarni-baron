using DotnetDispatcher.Core;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public record GetExchangeRateQuery(DateOnly Date) : IQuery<Result<GetExchangeRateQueryResponse>>;
