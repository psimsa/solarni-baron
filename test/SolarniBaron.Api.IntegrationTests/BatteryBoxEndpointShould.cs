namespace SolarniBaron.Api.IntegrationTests;

public class BatteryBoxEndpointShould
{
    private readonly HttpClient _client;

    public BatteryBoxEndpointShould()
    {
        _client = TestHostBuilder.GetClient();

    }
    [Fact]
    public async Task GetStats()
    {
        Assert.True(false, "This test needs an implementation");
    }
}
