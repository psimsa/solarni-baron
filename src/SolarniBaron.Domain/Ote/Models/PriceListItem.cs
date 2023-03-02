namespace SolarniBaron.Domain.Ote.Models;

public record PriceListItem(
    int HourIndex,
    decimal BasePriceEur,
    decimal BasePriceCzk,
    decimal BasePriceCzkVat,
    decimal BasePriceCzkTotal,
    decimal WithSurchargeCzk,
    decimal WithSurchargeCzkVat,
    decimal WithSurchargeCzkTotal,
    decimal BuyPriceCzk,
    int PriceScore,
    int PriceCluster)
{
    public static PriceListItem Empty = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    // public int Hour => HourIndex + 1;
}
