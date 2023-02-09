using DotnetDispatcher.Core;
using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain.BatteryBox;

namespace SolarniBaron.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.RegisterSolarniBaronDispatcherAndHandlers();

        services.AddTransient<IBatteryBoxDataConnector, OigDataConnector>();

        return services;
    }
}
