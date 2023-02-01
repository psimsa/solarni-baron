namespace SolarniBaron.Domain.Contracts;

public interface IApiHttpClient : IDisposable
{
    Uri? BaseAddress { get; set; }
    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    Task<string> GetStringAsync(string url);
}
