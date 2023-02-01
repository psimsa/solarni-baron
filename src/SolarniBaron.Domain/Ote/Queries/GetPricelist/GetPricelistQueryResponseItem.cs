namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponseItem(int Hour, decimal BasePriceEur, decimal BasePriceCzk,
decimal BasePriceCzkVat,
decimal BasePriceCzkTotal,
decimal WithSurchargeCzk,
decimal WithSurchargeCzkVat,
decimal WithSurchargeCzkTotal)
{
    public static GetPricelistQueryResponseItem Empty = new(0, 0, 0, 0, 0, 0, 0, 0);
}
