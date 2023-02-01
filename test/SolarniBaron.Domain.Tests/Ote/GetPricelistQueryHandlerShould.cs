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

        _cacheMock.Setup(x => x.GetAsync("pricelist-2022-10-11", It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();

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
        Assert.Equal(24, response.Data.Items.Length);

        var responseItems = response.Data.Items;

        AssertWrapper.AssertAll(
            () => Assert.Equal(1, responseItems[0].Hour),
            () => Assert.Equal(78.57m, responseItems[0].RateEur),
            () => Assert.Equal(1926.53640m, responseItems[0].RateCzk),
            () => Assert.Equal(2226.53640m, responseItems[0].WithSurchargeCzk),
            () => Assert.Equal(467.572644m, responseItems[0].VatCzk),
            () => Assert.Equal(2694.109044m, responseItems[0].TotalCzk)
            );
    }

    [Fact]
    public async Task GetPricelistFromCache()
    {
        var cachedValue = new GetPricelistQueryResponse(new GetPricelistQueryResponseData(new[] { new GetPricelistQueryResponseItem(1, 2, 3, 4, 5, 6) }, 10.001m), ResponseStatus.Ok);
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cachedValue))).Verifiable();
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ThrowsAsync(new NotImplementedException()).Verifiable();

        var handler = new GetPricelistQueryHandler(_getExchangeRateQueryHandlerMock.Object, _httpClientMock.Object, _cacheMock.Object,
            Mock.Of<ILogger<GetPricelistQueryHandler>>());
        var response = await handler.Get(new GetPricelistQuery(new DateOnly(2022, 10, 10)));

        Assert.NotNull(response);
        _httpClientMock.VerifyNoOtherCalls();
        _cacheMock.VerifyAll();
        _getExchangeRateQueryHandlerMock.VerifyAll();
        Assert.Equal(10.001m, response.Data.ExchangeRate);
        Assert.Single(response.Data.Items);

        var responseItems = response.Data.Items;

        AssertWrapper.AssertAll(
            () => Assert.Equal(1, responseItems[0].Hour),
            () => Assert.Equal(2, responseItems[0].RateEur),
            () => Assert.Equal(3, responseItems[0].RateCzk),
            () => Assert.Equal(4, responseItems[0].WithSurchargeCzk),
            () => Assert.Equal(5, responseItems[0].VatCzk),
            () => Assert.Equal(6, responseItems[0].TotalCzk)
            );
    }
}
