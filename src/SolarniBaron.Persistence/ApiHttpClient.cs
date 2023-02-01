using Microsoft.Extensions.Logging;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Persistence;

public class ApiHttpClient : IApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiHttpClient> _logger;

    public ApiHttpClient(HttpClient httpClient, ILogger<ApiHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Uri? BaseAddress
    {
        get => _httpClient.BaseAddress;
        set => _httpClient.BaseAddress = value;
    }

    public Task<HttpResponseMessage> GetAsync(string url) => _httpClient.GetAsync(url);

    public Task<string> GetStringAsync(string url) => _httpClient.GetStringAsync(url);

    public Task<HttpResponseMessage> PostAsync(string url, HttpContent content) => _httpClient.PostAsync(url, content);

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage message) => _httpClient.SendAsync(message);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpClient.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
