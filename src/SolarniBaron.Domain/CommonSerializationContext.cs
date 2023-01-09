using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

namespace SolarniBaron.Domain;

[JsonSerializable(typeof(LoginInfo))]
[JsonSerializable(typeof(BatteryBoxUnits))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(BatteryBoxStatus))]
[JsonSerializable(typeof(ApiResponse<GetPricelistQueryResponse>))]
public partial class CommonSerializationContext : JsonSerializerContext
{
}

public interface IApiHttpClient : IDisposable
{
    Uri BaseAddress { get; set; }
    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    Task<string> GetStringAsync(string url);
}
public class ApiHttpClient : IApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiHttpClient> _logger;

    public ApiHttpClient(HttpClient httpClient, ILogger<ApiHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Uri BaseAddress { get; set; }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    public Task<HttpResponseMessage> GetAsync(string url) => _httpClient.GetAsync(url);

    public Task<string> GetStringAsync(string url) => _httpClient.GetStringAsync(url);

    public Task<HttpResponseMessage> PostAsync(string url, HttpContent content) => _httpClient.PostAsync(url, content);

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage message) => _httpClient.SendAsync(message);
}
