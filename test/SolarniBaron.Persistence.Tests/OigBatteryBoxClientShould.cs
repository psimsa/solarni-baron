using SolarniBaron.Persistence.BatteryBox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarniBaron.Domain;

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
}
