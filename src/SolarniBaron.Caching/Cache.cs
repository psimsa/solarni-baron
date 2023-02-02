using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Caching;

public class Cache : ICache
{
    private readonly IDistributedCache _cache;

    public Cache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public Task<byte[]?> GetAsync(string key, CancellationToken token = new CancellationToken()) => _cache.GetAsync(key, token);

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
        CancellationToken token = new CancellationToken()) => _cache.SetAsync(key, value, options, token);

    public Task RemoveAsync(string key, CancellationToken token = new CancellationToken()) => _cache.RemoveAsync(key, token);

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> createItem, DistributedCacheEntryOptions? options = null)
        where T : class
    {
        var cachedItem = await _cache.GetAsync(key);
        if (cachedItem is not null)
        {
            var toReturn = JsonSerializer.Deserialize<T>(Decompress(cachedItem));
            return toReturn;
        }

        var item = await createItem();
        if (item is not null)
        {
            var serializedItem = Compress(JsonSerializer.Serialize(item));
            await _cache.SetAsync(key, serializedItem, options ?? new DistributedCacheEntryOptions());
        }

        return item;
    }

    private byte[] Compress(string input)
    {
        using var ms = new MemoryStream();
        using var gs = new GZipStream(ms, CompressionMode.Compress);
        using var sw = new StreamWriter(gs);
        sw.Write(input);
        return ms.ToArray();
    }

    private string Decompress(byte[] input)
    {
        using var ms = new MemoryStream(input);
        using var gs = new GZipStream(ms, CompressionMode.Decompress);
        using var sr = new StreamReader(gs);
        return sr.ReadToEnd();
    }

    public async Task<string?> GetOrCreateAsync(string key, Func<Task<string?>> createItem, DistributedCacheEntryOptions? options = null)
    {
        var cachedItem = await _cache.GetAsync(key);
        if (cachedItem is not null)
        {
            string toReturn = Encoding.UTF8.GetString(cachedItem);
            return toReturn;
        }

        var item = await createItem();
        if (item is not null)
        {
            var itemAsBytes = Encoding.UTF8.GetBytes(item);
            await _cache.SetAsync(key, itemAsBytes, options ?? new DistributedCacheEntryOptions());
        }

        return item;
    }
}
