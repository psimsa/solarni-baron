using System.Text.Json;
using Microsoft.JSInterop;

namespace SolarniBaron.Web.Core;

public interface IStorage
{
    Task<string?> GetItem(string key);
    Task<T?> GetItem<T>(string key);
    Task SetItem(string key, string value);
    Task SetItem<T>(string key, T value);
}

public class LocalStorage : IStorage
{
    private readonly IJSRuntime _js;

    public LocalStorage(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string?> GetItem(string key)
    {
        return await _js.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task<T?> GetItem<T>(string key)
    {
        var item = await GetItem(key);
        return item == null ? default : JsonSerializer.Deserialize<T>(item);
    }

    public async Task SetItem(string key, string value)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task SetItem<T>(string key, T value)
    {
        var item = JsonSerializer.Serialize(value);
        await SetItem(key, item);
    }
}
