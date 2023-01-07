namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public record GetExchangeRateQueryResponse(decimal Rate)
{
    public static GetExchangeRateQueryResponse Empty() => new(0);
};
