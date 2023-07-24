using OteSoapService;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Services;

public class OteService : IOteService
{
    public async Task<decimal[]> GetPricesForDay(DateTime date)
    {
        using var ote = new PublicDataServiceSoapClient();

        var result = await ote.GetDamPriceEAsync(date, date, 0, 24, true);
        return result.Result.Select(x => x.Price).ToArray();
    }
}
