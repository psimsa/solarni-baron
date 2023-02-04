using DotnetDispatcher.Core;
using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

namespace SolarniBaron.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<ISolarniBaronDispatcher, SolarniBaronDispatcher>();

        services.AddTransient<IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse>, GetPricelistQueryHandler>();
        services.AddTransient<IQueryHandler<GetExchangeRateQuery, Result<GetExchangeRateQueryResponse>>, GetExchangeRateQueryHandler>();
        services.AddTransient<IQueryHandler<GetStatsQuery, GetStatsQueryResponse>, GetStatsQueryHandler>();
        services.AddTransient<ICommandHandler<SetModeCommand, SetModeCommandResponse>, SetModeCommandHandler>();
        services.AddTransient<IQueryHandler<GetPriceOutlookQuery, GetPriceOutlookQueryResponse>,
                GetPriceOutlookQueryHandler>();


        services.AddTransient<IBatteryBoxDataConnector, OigDataConnector>();

        return services;
    }
}
