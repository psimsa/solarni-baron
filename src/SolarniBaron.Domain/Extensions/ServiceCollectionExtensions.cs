using DotnetDispatcher.Core;
using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.Clustering;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Services;

namespace SolarniBaron.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.RegisterSolarniBaronDispatcherAndHandlers();

        services.AddSingleton(typeof(IPriceClusteringWorker), typeof(PriceClusteringWorker));

        services.AddTransient<IBatteryBoxDataConnector, OigDataConnector>();

        services.AddSingleton<IOteService, OteService>();

        return services;
    }
}
