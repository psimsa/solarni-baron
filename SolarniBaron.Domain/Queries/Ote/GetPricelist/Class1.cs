using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Queries.Ote.GetPricelist;

public record GetPricelistQuery(DateOnly date) : IQuery<GetPricelistQueryResponse>;

public record GetPricelistQueryResponse()
{
    public static GetPricelistQueryResponse Empty() => new GetPricelistQueryResponse();
};