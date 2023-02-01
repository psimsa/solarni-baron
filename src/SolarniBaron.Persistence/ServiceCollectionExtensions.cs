using Microsoft.Extensions.DependencyInjection;
using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Persistence.BatteryBox;

namespace SolarniBaron.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddTransient<IBatteryBoxClient, OigBatteryBoxClient>();
        services.AddHttpClient<IApiHttpClient, ApiHttpClient>();

        return services;
    }
}
