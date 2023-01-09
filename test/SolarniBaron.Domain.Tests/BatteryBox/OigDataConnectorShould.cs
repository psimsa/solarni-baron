using Microsoft.Extensions.Logging;
using Moq;
using SolarniBaron.Domain.BatteryBox;

namespace SolarniBaron.Domain.Tests;

public class OigDataConnectorShould
{
    private readonly Mock<IBatteryBoxClient> _client;
    private readonly Mock<ILogger<OigDataConnector>> _logger;

    public OigDataConnectorShould()
    {

        _client = new Mock<IBatteryBoxClient>();
        _logger = new Mock<ILogger<OigDataConnector>>();
    }

    [Fact]
    public async Task GetRawStats()
    {
        var connector = new OigDataConnector(_client.Object, _logger.Object);
        Assert.True(false, "This test needs an implementation");
    }
    [Fact]
    public async Task GetStatsForUnit()
    {
        var connector = new OigDataConnector(_client.Object, _logger.Object);
        Assert.True(false, "This test needs an implementation");
    }
    [Fact]
    public async Task SetMode()
    {
        var connector = new OigDataConnector(_client.Object, _logger.Object);
        Assert.True(false, "This test needs an implementation");
    }
}
