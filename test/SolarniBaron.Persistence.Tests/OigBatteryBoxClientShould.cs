using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using SolarniBaron.Domain;
using SolarniBaron.Persistence.BatteryBox;

namespace SolarniBaron.Persistence.Tests;

public class OigBatteryBoxClientShould
{
    private readonly Mock<IApiHttpClient> _apiHttpClientMock;

    public OigBatteryBoxClientShould()
    {
        _apiHttpClientMock = new Mock<IApiHttpClient>();
    }

    [Fact]
    public void Construct()
    {
        var client = new OigBatteryBoxClient(_apiHttpClientMock.Object, NullLogger<OigBatteryBoxClient>.Instance);
    }

    [Fact]
    public async Task GetRawStats()
    {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact]
    public async Task SetMode()
    {
        Assert.True(false, "This test needs an implementation");
    }
}
