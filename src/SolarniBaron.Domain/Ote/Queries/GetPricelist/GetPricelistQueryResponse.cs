using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponse(ResponseStatus ResponseStatus, GetPricelistQueryResponseItem[] Items,
    decimal ExchangeRate)
{
    public static GetPricelistQueryResponse Empty() =>
        new(ResponseStatus.Error, Array.Empty<GetPricelistQueryResponseItem>(), 0);
};
