using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using SolarniBaron.Caching;
using SolarniBaron.Domain.Clustering;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using TestHelpers;
using TestHelpers.TestData;

namespace SolarniBaron.Domain.Tests.Ote;

public class GetPricelistQueryHandlerShould
{
    private readonly Mock<IOteService> _oteServiceMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<ISolarniBaronDispatcher> _dispatcherMock;

    public GetPricelistQueryHandlerShould()
    {
        _oteServiceMock = new Mock<IOteService>();
        _cacheMock = new Mock<IDistributedCache>();
        _dispatcherMock = new Mock<ISolarniBaronDispatcher>();
    }

    [Fact]
    public async Task GetPricelist()
    {
        _oteServiceMock.Setup(x => x.GetPricesForDay(It.IsAny<DateTime>()))
            .ReturnsAsync(
            new decimal[] { 78.57m, 77.21m, 83.23m, 77.21m, 78.86m, 141.40m, 230.96m, 227.26m, 269.64m, 185.21m, 159.46m, 152.80m, 145.25m, 172.46m, 195.14m, 205.03m, 213.93m, 248.12m, 268.72m, 308.72m, 230.00m, 194.36m, 173.54m, 159.34m }
                )
            .Verifiable();

        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();

        _dispatcherMock.Setup(x => x.Dispatch(It.IsAny<GetExchangeRateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<GetExchangeRateQueryResponse>(ResponseStatus.Ok, new GetExchangeRateQueryResponse(24.520m)))
            .Verifiable();

        var handler = new GetPricelistQueryHandler(_dispatcherMock.Object, new PriceClusteringWorker(), _oteServiceMock.Object,
            new Cache(_cacheMock.Object),
            Mock.Of<ILogger<GetPricelistQueryHandler>>());
        var response = await handler.Query(new GetPricelistQuery(new DateOnly(2022, 10, 11)), CancellationToken.None);

        Assert.NotNull(response);
        _oteServiceMock.VerifyAll();
        _cacheMock.VerifyAll();
        _dispatcherMock.VerifyAll();
        Assert.Equal(24.520m, response.ExchangeRate);
        Assert.Equal(24, response.HourlyRateBreakdown.Length);

        var responseItems = response.HourlyRateBreakdown;

        AssertWrapper.AssertAll(
            () => Assert.Equal(0, responseItems[0].HourIndex),
            () => Assert.Equal(78.57m, responseItems[0].BasePriceEur),
            () => Assert.Equal(1926.53640m, responseItems[0].BasePriceCzk),
            () => Assert.Equal(2226.53640m, responseItems[0].WithSurchargeCzk),
            () => Assert.Equal(467.572644m, responseItems[0].WithSurchargeCzkVat),
            () => Assert.Equal(2694.109044m, responseItems[0].WithSurchargeCzkTotal)
        );
    }

    [Fact]
    public async Task GetPricelistFromCache()
    {
        var cachedValue =
            "78.57|77.21|83.23|77.21|78.86|141.40|230.96|227.26|269.64|185.21|159.46|152.80|145.25|172.46|195.14|205.03|213.93|248.12|268.72|308.72|230.00|194.36|173.54|159.34";
        _cacheMock.Setup(x => x.GetAsync("Cq1wD0oQ+s2Eeu7cbLDGahJst6o=", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes(cachedValue)).Verifiable();
        _oteServiceMock.Setup(x => x.GetPricesForDay(It.IsAny<DateTime>())).ThrowsAsync(new NotImplementedException()).Verifiable();
        _dispatcherMock.Setup(x => x.Dispatch(It.IsAny<GetExchangeRateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<GetExchangeRateQueryResponse>(ResponseStatus.Ok, new GetExchangeRateQueryResponse(24.520m)))
            .Verifiable();

        var handler = new GetPricelistQueryHandler(_dispatcherMock.Object, new PriceClusteringWorker(), _oteServiceMock.Object,
            new Cache(_cacheMock.Object),
            Mock.Of<ILogger<GetPricelistQueryHandler>>());
        var response = await handler.Query(new GetPricelistQuery(new DateOnly(2022, 10, 10)), CancellationToken.None);

        Assert.NotNull(response);
        _oteServiceMock.VerifyNoOtherCalls();
        _cacheMock.VerifyAll();
        _dispatcherMock.VerifyAll();

        Assert.NotNull(response.HourlyRateBreakdown);
        Assert.Equal(24.520m, response.ExchangeRate);
        Assert.Equal(24, response.HourlyRateBreakdown.Length);

        var responseItems = response.HourlyRateBreakdown;

        AssertWrapper.AssertAll(
            () => Assert.Equal(0, responseItems[0].HourIndex),
            () => Assert.Equal(78.57m, responseItems[0].BasePriceEur),
            () => Assert.Equal(1926.53640m, responseItems[0].BasePriceCzk),
            () => Assert.Equal(2226.53640m, responseItems[0].WithSurchargeCzk),
            () => Assert.Equal(467.572644m, responseItems[0].WithSurchargeCzkVat),
            () => Assert.Equal(2694.109044m, responseItems[0].WithSurchargeCzkTotal)
        );
    }
}
