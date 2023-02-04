using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public record GetExchangeRateQueryResponse(decimal Rate, ResponseStatus ResponseStatus = ResponseStatus.Ok,
    string? Error = null) 
{
    public static GetExchangeRateQueryResponse Empty() => new(0, ResponseStatus.Empty);
}
