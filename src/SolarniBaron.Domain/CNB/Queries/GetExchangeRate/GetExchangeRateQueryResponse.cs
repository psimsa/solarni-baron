using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public record GetExchangeRateQueryResponse(decimal Rate, ResponseStatus ResponseStatus = ResponseStatus.Ok,
    string? Error = null) : QueryResponse<decimal>(Rate, ResponseStatus, Error)
{
    public static GetExchangeRateQueryResponse Empty() => new(0, ResponseStatus.Empty);
}
