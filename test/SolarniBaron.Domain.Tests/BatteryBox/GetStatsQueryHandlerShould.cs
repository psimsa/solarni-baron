﻿using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.Contracts;
using TestHelpers;
using TestHelpers.TestData;

namespace SolarniBaron.Domain.Tests.BatteryBox;

public class GetStatsQueryHandlerShould
{
    private readonly Mock<IBatteryBoxDataConnector> _batteryBoxDataConnectorMock;
    private readonly Mock<ICache> _cacheMock;

    public GetStatsQueryHandlerShould()
    {
        _batteryBoxDataConnectorMock = new Mock<IBatteryBoxDataConnector>();
        _cacheMock = new Mock<ICache>();
    }

    [Fact]
    public async Task GetStats()
    {
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();


        var returnedObject = JsonSerializer.Deserialize(OigResponses.GetRawStatsResponse,
            CommonSerializationContext.Default.DictionaryStringBatteryBoxUnitData);
        _batteryBoxDataConnectorMock.Setup(_ => _.GetStatsForUnit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
            .ReturnsAsync(returnedObject!.First().Value with {UnitId = "asdf"}).Verifiable();


        var handler = new GetStatsQueryHandler(_batteryBoxDataConnectorMock.Object, _cacheMock.Object,
            NullLogger<GetStatsQueryHandler>.Instance);

        var response = await handler.Query(new GetStatsQuery("hello", "world", "asdf"));

        Assert.NotNull(response);
        var status = response.BatteryBoxStatus;
        AssertWrapper.AssertAll(
            () => Assert.Equal("asdf", status.UnitId),
            () => Assert.Equal(33, status.BatteryPct),
            () => Assert.Equal(0, status.ConsumptionL1),
            () => Assert.Equal(66, status.ConsumptionL2),
            () => Assert.Equal(19, status.ConsumptionL3),
            () => Assert.Equal(Domain.BatteryBox.Models.BatteryBox.OperationMode.Home1, status.OperationMode)
        );
    }
}
