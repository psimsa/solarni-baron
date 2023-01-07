using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Persistence.BatteryBox;
using TestHelpers.TestData;

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

    [Fact]
    public async Task GetRawStats()
    {
        SetupValidAuthentication(_apiHttpClientMock);
        _apiHttpClientMock.Setup(_ => _.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(OigResponses.GetRawStatsResponse, new MediaTypeHeaderValue("application/json"))
        });

        var client = new OigBatteryBoxClient(_apiHttpClientMock.Object, NullLogger<OigBatteryBoxClient>.Instance);
        var rawStats = await client.GetRawStats("hello", "world");
        Assert.Equal(OigResponses.GetRawStatsResponse, rawStats);
    }

    [Fact]
    public async Task Throw_IfAuthenticationFails()
    {
        SetupInvalidAuthentication(_apiHttpClientMock);
        var client = new OigBatteryBoxClient(_apiHttpClientMock.Object, NullLogger<OigBatteryBoxClient>.Instance);
        await Assert.ThrowsAsync<AuthenticationException>(() => client.GetRawStats("hello", "world"));
    }

    [Fact]
    public async Task SetMode()
    {
        SetupValidAuthentication(_apiHttpClientMock);
        _apiHttpClientMock.Setup(_ => _.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(
            (string url, HttpContent content) =>
            {
                var data = content.ReadFromJsonAsync<JsonNode>().Result;
                if (data!["value"]?.ToString() == "1"
                    && data["id_device"]?.ToString() == "asdf"
                    && data["table"]?.ToString() == "box_prms"
                    && data["column"]?.ToString() == "mode")
                {
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent("Ok") };
                }

                throw new ArgumentException(nameof(content));
            });

        var client = new OigBatteryBoxClient(_apiHttpClientMock.Object, NullLogger<OigBatteryBoxClient>.Instance);
        var (result, message) = await client.SetMode("hello", "world", "asdf", "1");
        Assert.True(result);
        Assert.Null(message);
    }

    [Fact]
    public async Task NotSetMode_IfAuthenticationFails()
    {
        SetupInvalidAuthentication(_apiHttpClientMock);
        var client = new OigBatteryBoxClient(_apiHttpClientMock.Object, NullLogger<OigBatteryBoxClient>.Instance);
        await Assert.ThrowsAsync<AuthenticationException>(() => client.SetMode("hello", "world", "asdf", "1"));
    }

    [Fact]
    public async Task NotSetMode_GivenInvalidMode()
    {
        SetupValidAuthentication(_apiHttpClientMock);
        _apiHttpClientMock.Setup(_ => _.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(
            new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Something wrong happened") });

        var client = new OigBatteryBoxClient(_apiHttpClientMock.Object, NullLogger<OigBatteryBoxClient>.Instance);
        var (result, message) = await client.SetMode("hello", "world", "asdf", "4");
        Assert.False(result);
        Assert.Equal("Something wrong happened", message);
    }

    private void SetupValidAuthentication(Mock<IApiHttpClient> client)
    {
        client.Setup(_ => _.SendAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Here be dragons") });
    }

    private void SetupInvalidAuthentication(Mock<IApiHttpClient> client)
    {
        client.Setup(_ => _.SendAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") });
    }
}
