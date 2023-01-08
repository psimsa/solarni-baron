namespace SolarniBaron.Api.IntegrationTests;

public class OkEndpointShould
{
    private readonly HttpClient _client;

    public OkEndpointShould()
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