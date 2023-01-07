using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponse(ResponseStatus Status, GetPricelistQueryResponseItem[] Items,
    decimal ExchangeRate)
{
    public static GetPricelistQueryResponse Empty() =>
        new GetPricelistQueryResponse(ResponseStatus.Error, Array.Empty<GetPricelistQueryResponseItem>(), 0);
};
