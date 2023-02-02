using Microsoft.Extensions.Caching.Distributed;

namespace SolarniBaron.Domain.Contracts;

public interface ICache
{
    Task<byte[]?> GetAsync(string key, CancellationToken token = new CancellationToken());
    Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = new CancellationToken());
    Task RemoveAsync(string key, CancellationToken token = new CancellationToken());
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> createItem, DistributedCacheEntryOptions? options = null) where T : class;
    Task<string?> GetOrCreateAsync(string key, Func<Task<string?>> createItem, DistributedCacheEntryOptions? options = null);
}
