using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox.Models;
using TestHelpers.TestData;

namespace SolarniBaron.Api.IntegrationTests;

public class BatteryBoxEndpointShould
{
    private readonly HttpClient _client;
    private readonly Mock<IApiHttpClient> _apiClientMock;
    private readonly Mock<IDistributedCache> _cacheMock;

    public BatteryBoxEndpointShould()
    {
        _apiClientMock = new Mock<IApiHttpClient>();
        _cacheMock = new Mock<IDistributedCache>();

        _client = TestHostBuilder.GetClient(services =>
        {
            services.AddSingleton<IApiHttpClient>(_apiClientMock.Object);
            services.AddSingleton(_cacheMock.Object);
        });

    }
    [Fact]
    public async Task GetStats()
    {
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();

        _apiClientMock.Setup(_ => _.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(OigResponses.GetRawStatsResponse, new MediaTypeHeaderValue("application/json"))
        }).Verifiable();
        _apiClientMock.Setup(_ => _.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[[2,\"\"]]", new MediaTypeHeaderValue("application/json"))
        }).Verifiable();

        var requestBodyJson = JsonSerializer.Serialize(new LoginInfo("hello", "world"));
        var response = await _client.PostAsync("api/batterybox/getstats", new StringContent(requestBodyJson, new MediaTypeHeaderValue("application/json")));

        _apiClientMock.VerifyAll();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task NotGetStats_IfAuthenticationFails()
    {
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as byte[]).Verifiable();

        _apiClientMock.Setup(_ => _.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("", new MediaTypeHeaderValue("application/json"))
        }).Verifiable();

        var requestBodyJson = JsonSerializer.Serialize(new LoginInfo("hello", "world"));
        var response = await _client.PostAsync("api/batterybox/getstats", new StringContent(requestBodyJson, new MediaTypeHeaderValue("application/json")));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal("\"Could not authenticate with OIG server\"", responseBody);
    }
}
