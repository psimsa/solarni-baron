using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

namespace SolarniBaron.Domain.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetOrCreateAsync<T>(this IDistributedCache cache, string key, Func<Task<T?>> createItem, DistributedCacheEntryOptions? options = null) where T : class
    {
        var cachedItem = await cache.GetAsync(key);
        if (cachedItem is not null)
        {
            var toReturn = JsonSerializer.Deserialize<T>(cachedItem);
            return toReturn;
        }
        var item = await createItem();
        if (item is not null)
        {
            var serializedItem = JsonSerializer.SerializeToUtf8Bytes(item);
            await cache.SetAsync(key, serializedItem, options ?? new DistributedCacheEntryOptions());
        }

        return item;
    }
    public static async Task<string?> GetOrCreateAsync(this IDistributedCache cache, string key, Func<Task<string?>> createItem, DistributedCacheEntryOptions? options = null)
    {
        var cachedItem = await cache.GetAsync(key);
        if (cachedItem is not null)
        {
            string toReturn = Encoding.UTF8.GetString(cachedItem);
            return toReturn;
        }
        var item = await createItem();
        if (item is not null)
        {
            var itemAsBytes = Encoding.UTF8.GetBytes(item);
            await cache.SetAsync(key, itemAsBytes, options ?? new DistributedCacheEntryOptions());
        }

        return item;
    }
}
