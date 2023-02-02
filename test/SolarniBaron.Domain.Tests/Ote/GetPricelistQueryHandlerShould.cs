using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using TestHelpers;
using TestHelpers.TestData;

namespace SolarniBaron.Domain.Tests.Ote;

public class GetPricelistQueryHandlerShould
{
    private readonly Mock<IApiHttpClient> _httpClientMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse>> _getExchangeRateQueryHandlerMock;

    public GetPricelistQueryHandlerShould()
    {
        _httpClientMock = new Mock<IApiHttpClient>();
        _cacheMock = new Mock<IDistributedCache>();
        _getExchangeRateQueryHandlerMock = new Mock<IQueryHandler<GetExchangeRateQuery, GetExchangeRateQueryResponse>>();
    }

    [Fact]
    public async Task GetPricelist()
    {
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>()))
            .ReturnsAsync(OteResponses.ValidPricelistResponse).Verifiable();

        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();

        _getExchangeRateQueryHandlerMock.Setup(x => x.Get(It.IsAny<IQuery<GetExchangeRateQuery, GetExchangeRateQueryResponse>>()))
            .ReturnsAsync(new GetExchangeRateQueryResponse(24.520m)).Verifiable();

        var handler = new GetPricelistQueryHandler(_getExchangeRateQueryHandlerMock.Object, _httpClientMock.Object, _cacheMock.Object,
            Mock.Of<ILogger<GetPricelistQueryHandler>>());
        var response = await handler.Get(new GetPricelistQuery(new DateOnly(2022, 10, 11)));

        Assert.NotNull(response);
        _httpClientMock.VerifyAll();
        _cacheMock.VerifyAll();
        _getExchangeRateQueryHandlerMock.VerifyAll();
        Assert.Equal(24.520m, response.Data.ExchangeRate);
        Assert.Equal(24, response.Data.HourlyRateBreakdown.Length);

        var responseItems = response.Data.HourlyRateBreakdown;

        AssertWrapper.AssertAll(
            () => Assert.Equal(1, responseItems[0].Hour),
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
        _cacheMock.Setup(x => x.GetAsync("Cq1wD0oQ+s2Eeu7cbLDGahJst6o=", It.IsAny<CancellationToken>())).ReturnsAsync(Encoding.UTF8.GetBytes(cachedValue)).Verifiable();
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ThrowsAsync(new NotImplementedException()).Verifiable();
        _getExchangeRateQueryHandlerMock.Setup(x => x.Get(It.IsAny<IQuery<GetExchangeRateQuery, GetExchangeRateQueryResponse>>()))
            .ReturnsAsync(new GetExchangeRateQueryResponse(24.520m)).Verifiable();

        var handler = new GetPricelistQueryHandler(_getExchangeRateQueryHandlerMock.Object, _httpClientMock.Object, _cacheMock.Object,
            Mock.Of<ILogger<GetPricelistQueryHandler>>());
        var response = await handler.Get(new GetPricelistQuery(new DateOnly(2022, 10, 10)));

        Assert.NotNull(response);
        _httpClientMock.VerifyNoOtherCalls();
        _cacheMock.VerifyAll();
        _getExchangeRateQueryHandlerMock.VerifyAll();
        Assert.Equal(24.520m, response.Data.ExchangeRate);
        Assert.Equal(24, response.Data.HourlyRateBreakdown.Length);

        var responseItems = response.Data.HourlyRateBreakdown;

        AssertWrapper.AssertAll(
            () => Assert.Equal(1, responseItems[0].Hour),
            () => Assert.Equal(78.57m, responseItems[0].BasePriceEur),
            () => Assert.Equal(1926.53640m, responseItems[0].BasePriceCzk),
            () => Assert.Equal(2226.53640m, responseItems[0].WithSurchargeCzk),
            () => Assert.Equal(467.572644m, responseItems[0].WithSurchargeCzkVat),
            () => Assert.Equal(2694.109044m, responseItems[0].WithSurchargeCzkTotal)
            );
    }
}
