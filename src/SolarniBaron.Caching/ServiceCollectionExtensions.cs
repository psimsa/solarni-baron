using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Caching.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tingle.Extensions.Caching.MongoDB;

namespace SolarniBaron.Caching;
public static class ServiceCollectionExtensions
{
    public static void RegisterCache(this IServiceCollection serviceCollection, IConfiguration configurationManager)
    {
        var cacheConfiguration = configurationManager.GetSection("CacheConfiguration");
        var cacheProvider = cacheConfiguration["Provider"] ?? "Memory";

        switch (cacheProvider)
        {
            case "Memory":
                serviceCollection.AddDistributedMemoryCache();
                break;
            case "Cosmos":
                var cosmosConfiguration = cacheConfiguration.GetSection("ProviderConfiguration");
                serviceCollection.AddCosmosCache((CosmosCacheOptions cacheOptions) =>
                {
                    cacheOptions.ContainerName = cosmosConfiguration["Container"];
                    cacheOptions.DatabaseName = cosmosConfiguration["Database"];
                    cacheOptions.ClientBuilder = new CosmosClientBuilder(cosmosConfiguration["ConnectionString"]);
                    cacheOptions.CreateIfNotExists = true;
                });
                break;
            case "Mongo":
                var mongoConfiguration = cacheConfiguration.GetSection("ProviderConfiguration");
                serviceCollection.AddMongoCache((MongoCacheOptions cacheOptions) =>
                {
                    cacheOptions.CollectionName = mongoConfiguration["Container"];
                    cacheOptions.DatabaseName = mongoConfiguration["Database"];
                    cacheOptions.ConnectionString = mongoConfiguration["ConnectionString"];
                    cacheOptions.CreateIfNotExists = true;
                });
                break;
            default:
                throw new ArgumentException($"Unknown cache provider: {cacheProvider}");
        }
    }
}
