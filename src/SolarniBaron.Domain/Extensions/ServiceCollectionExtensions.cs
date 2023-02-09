using DotnetDispatcher.Core;
using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.Clustering;

namespace SolarniBaron.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.RegisterSolarniBaronDispatcherAndHandlers();

        services.AddSingleton(typeof(IPriceClusteringWorker), typeof(PriceClusteringWorker));

        services.AddTransient<IBatteryBoxDataConnector, OigDataConnector>();

        return services;
    }
}
