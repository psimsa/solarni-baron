using System.Globalization;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using SolarniBaron.Caching;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Contracts;
using TestHelpers.TestData;

namespace SolarniBaron.Domain.Tests.CNB;

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

        var handler = new GetExchangeRateQueryHandler(_httpClientMock.Object, new Cache(_cacheMock.Object),
            Mock.Of<ILogger<GetExchangeRateQueryHandler>>());
        var response = await handler.Query(new GetExchangeRateQuery(new DateOnly(2022, 10, 10)));

        Assert.NotNull(response);
        _httpClientMock.VerifyAll();
        _cacheMock.VerifyAll();
        Assert.Equal(24.520m, response.Data.Rate);
    }

    [Fact]
    public async Task GetExchangeRateFromCache()
    {
        var cachedValue = new GetExchangeRateQueryResponse(24.520m);

        _cacheMock.Setup(x => x.GetAsync("CcyEtU0Mnp6xjGB/qM30GaETyQ8=", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes(cachedValue.Rate.ToString(CultureInfo.InvariantCulture))).Verifiable();
        _httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ThrowsAsync(new NotImplementedException()).Verifiable();
        var handler = new GetExchangeRateQueryHandler(_httpClientMock.Object, new Cache(_cacheMock.Object),
            Mock.Of<ILogger<GetExchangeRateQueryHandler>>());
        var response = await handler.Query(new GetExchangeRateQuery(new DateOnly(2022, 10, 11)));

        Assert.NotNull(response);
        _httpClientMock.VerifyNoOtherCalls();

        _cacheMock.VerifyAll();
        Assert.Equal(24.520m, response.Data.Rate);
    }
}
