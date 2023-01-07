using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts.Commands;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

namespace SolarniBaron.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>, GetPricelistQueryHandler>();
        services
            .AddTransient<IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse>,
                GetExchangeRateQueryHandler>();
        services.AddTransient<IQueryHandler<GetStatsQuery, GetStatsQueryResponse>, GetStatsQueryHandler>();
        services.AddTransient<ICommandHandler<SetModeCommand, SetModeCommandResponse>, SetModeCommandHandler>();

        services.AddTransient<IBatteryBoxDataConnector, OigDataConnector>();

        return services;
    }
}
