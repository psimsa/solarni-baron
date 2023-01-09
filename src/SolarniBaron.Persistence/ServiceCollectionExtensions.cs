using Microsoft.Extensions.DependencyInjection;

using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Persistence.BatteryBox;

namespace SolarniBaron.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddHttpClient<IBatteryBoxClient, OigBatteryBoxClient>();
        return services;
    }
}
