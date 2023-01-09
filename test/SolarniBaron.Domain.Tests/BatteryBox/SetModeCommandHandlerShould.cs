using Microsoft.Extensions.Logging;
using Moq;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;

namespace SolarniBaron.Domain.Tests;

public class SetModeCommandHandlerShould
{
    private Mock<IBatteryBoxDataConnector> _connector;
    private Mock<ILogger<SetModeCommandHandler>> _logger;

    public SetModeCommandHandlerShould()
    {
        _connector = new Mock<IBatteryBoxDataConnector>();
        _logger = new Mock<ILogger<SetModeCommandHandler>>();
    }

    [Fact]
    public async Task SetMode()
    {
        var handler = new SetModeCommandHandler(_connector.Object, _logger.Object);
        Assert.True(false, "This test needs an implementation");
    }
}
