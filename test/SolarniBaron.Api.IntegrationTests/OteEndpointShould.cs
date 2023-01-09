using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using SolarniBaron.Domain;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

using TestHelpers.TestData;

namespace SolarniBaron.Api.IntegrationTests;

public class OteEndpointShould
{
    private readonly HttpClient _client;
    private readonly Mock<IApiHttpClient> _apiClientMock;
    private readonly Mock<IDistributedCache> _cacheMock;


    public OteEndpointShould()
    {
        _apiClientMock = new Mock<IApiHttpClient>();
        _cacheMock = new Mock<IDistributedCache>();

        _client = TestHostBuilder.GetClient(services =>
        {
            services.AddSingleton(_apiClientMock.Object);
            services.AddSingleton(_cacheMock.Object);
        });
    }

    [Fact]
    public async Task ReturnPricelist_GivenDate()
    {
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();
        _apiClientMock.Setup(x => x.GetStringAsync("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date=10.10.2022")).ReturnsAsync(CnbResponses.ValidExchangeRateResponse).Verifiable();
        _apiClientMock.Setup(x => x.GetStringAsync("https://www.ote-cr.cz/cs/kratkodobe-trhy/elektrina/denni-trh/?date=2022-10-10")).ReturnsAsync(OteResponses.ValidPricelistResponse).Verifiable();

        var expectedObject = JsonSerializer.Deserialize(GetPricelistResponses.GetPricelistResponse, CommonSerializationContext.Default.ApiResponseGetPricelistQueryResponse);

        var response = await _client.GetAsync("api/ote/2022-10-10");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize(responseString, CommonSerializationContext.Default.ApiResponseGetPricelistQueryResponse);

        Assert.Equal(expectedObject, responseObject);
    }
}
