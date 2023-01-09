using System.Net.Http.Json;

namespace SolarniBaron.Api.IntegrationTests;

public class HealthzEndpointShould
{
    private readonly HttpClient _client;

    public HealthzEndpointShould()
    {
        _client = TestHostBuilder.GetClient();
    }

    [Fact]
    public async Task ReturnOk()
    {
        var response = await _client.GetAsync("/healthz");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("OK", responseString);
    }
}
