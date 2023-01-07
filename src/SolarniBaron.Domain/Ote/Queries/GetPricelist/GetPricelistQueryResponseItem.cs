namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponseItem(int Hour, decimal RateEur, decimal RateCzk,
    decimal WithSurchargeCzk, decimal VatCzk, decimal TotalCzk)
{
    public static GetPricelistQueryResponseItem Empty => new(0, 0, 0, 0, 0, 0);
}
