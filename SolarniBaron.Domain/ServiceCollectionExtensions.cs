using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

namespace SolarniBaron.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>, GetPricelistQueryHandler>();
        services
            .AddTransient<IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse>,
                GetExchangeRateQueryHandler>();

        return services;
    }
}
