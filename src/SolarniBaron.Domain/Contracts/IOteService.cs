namespace SolarniBaron.Domain.Contracts;

public interface IOteService
{
    Task<decimal[]> GetPricesForDay(DateTime date);
}
