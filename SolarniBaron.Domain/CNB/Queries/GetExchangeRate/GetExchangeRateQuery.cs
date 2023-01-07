using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public record GetExchangeRateQuery(DateOnly date) : IQuery<GetExchangeRateQueryResponse>;