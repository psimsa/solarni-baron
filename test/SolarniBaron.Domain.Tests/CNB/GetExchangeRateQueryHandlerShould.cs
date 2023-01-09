using System.Net.Http;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

using Moq;

using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

using TestHelpers.TestData;

namespace SolarniBaron.Domain.Tests;

public class GetExchangeRateQueryHandlerShould
{
    private readonly Mock<IApiHttpClient> _httpClientMock;
    private readonly Mock<IDistributedCache> _cacheMock;

    public GetExchangeRateQueryHandlerShould()
    {
        _httpClientMock = new Mock<IApiHttpClient>();
        _cacheMock = new Mock<IDistributedCache>();
    }

    [Fact]
    public async Task GetExchangeRate()
    {
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>()))
            .ReturnsAsync(CnbResponses.ValidExchangeRateResponse).Verifiable();

        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();

        var handler = new GetExchangeRateQueryHandler(_httpClientMock.Object, _cacheMock.Object);
        var response = await handler.Get(new GetExchangeRateQuery(new DateOnly(2022, 10, 10)));

        Assert.NotNull(response);
        _httpClientMock.VerifyAll();
        _cacheMock.VerifyAll();
        Assert.Equal(24.520m, response.Rate);
    }

    [Fact]
    public async Task GetExchangeRateFromCache()
    {
        var cachedValue = new GetExchangeRateQueryResponse(24.520m);
        _cacheMock.Setup(x => x.GetAsync("pricelist-2022-10-11", It.IsAny<CancellationToken>())).ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cachedValue))).Verifiable();
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ThrowsAsync(new NotImplementedException()).Verifiable();
        var handler = new GetExchangeRateQueryHandler(_httpClientMock.Object, _cacheMock.Object);
        var response = await handler.Get(new GetExchangeRateQuery(new DateOnly(2022, 10, 11)));

        Assert.NotNull(response);
        _httpClientMock.VerifyNoOtherCalls();

        _cacheMock.VerifyAll();
        Assert.Equal(24.520m, response.Rate);
    }
}
