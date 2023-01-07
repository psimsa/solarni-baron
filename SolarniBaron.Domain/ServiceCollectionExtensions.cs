using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

namespace SolarniBaron.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services
            .AddTransient<IQueryHandler<IQuery<GetExchangeRateQueryResponse>, GetExchangeRateQueryResponse>,
                GetExchangeRateQueryHandler>();
        services
            .AddTransient<IQueryHandler<IQuery<GetPricelistQueryResponse>, GetPricelistQueryResponse>,
                GetPricelistQueryHandler>();
        
        return services;
    }
}
