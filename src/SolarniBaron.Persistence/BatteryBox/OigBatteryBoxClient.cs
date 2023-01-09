using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox;
using SolarniBaron.Domain.BatteryBox.Models;

namespace SolarniBaron.Persistence.BatteryBox;

public partial class OigBatteryBoxClient : IBatteryBoxClient
{
    private readonly IApiHttpClient _client;
    private readonly ILogger<OigBatteryBoxClient> _logger;

    [LoggerMessage(1150, LogLevel.Debug, "Attempting to login...")]
    public partial void LogAttemptingToLogin();

    [LoggerMessage(1151, LogLevel.Debug, "Api call successful: {apiCallResult}")]
    public partial void LogApiCallSuccessful(string apiCallResult);

    [LoggerMessage(1350, LogLevel.Error, "Login failed.")]
    public partial void LogLoginFailed();

    [LoggerMessage(1351, LogLevel.Error, "Api call failed with code {code} and response {apiReponseString}")]
    public partial void LogApiCallFailed(int code, string apiReponseString);

    public OigBatteryBoxClient(IApiHttpClient client, ILogger<OigBatteryBoxClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string> GetRawStats(string username, string password)
    {
        await AuthenticateClient(username, password);

        var currentStateResponse =
            await _client.GetAsync(Constants.GetStatus + "?_nonce=" + DateTimeOffset.Now.ToUnixTimeMilliseconds());

        var currentState = await currentStateResponse.Content.ReadAsStringAsync();
        return currentState;
    }

    public async Task<(bool, string?)> SetMode(string username, string password, string unitId, string mode)
    {
        await AuthenticateClient(username, password);
        var command = new OigCommand(unitId, "box_prms", "mode", mode);

        var serializedCommand = JsonSerializer.Serialize(command);
        var content = new StringContent(serializedCommand);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response =
            await _client.PostAsync(Constants.SetValue + "?_nonce=" + DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                content);
        var responseBody = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            LogApiCallFailed((int)response.StatusCode, responseBody);
            return (false, responseBody);
        }

        LogApiCallSuccessful(responseBody);
        return (true, null);
    }

    private async Task AuthenticateClient(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username must be provided", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password must be provided", nameof(password));
        }

        _client.BaseAddress = new Uri(Constants.baseUrl);

        LogAttemptingToLogin();

        var loginInfo = new LoginInfo(username, password);

        var serializedLoginInfo =
            JsonSerializer.Serialize(loginInfo, PersistenceSerializationContext.Default.LoginInfo);

        var content = new StringContent(serializedLoginInfo);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var message = new HttpRequestMessage(HttpMethod.Post, Constants.loginUrl)
        {
            Content = content,
        };

        var loginInfoResponse = await _client.SendAsync(message);
        if (loginInfoResponse.Content.Headers.ContentLength == 0)
        {
            LogLoginFailed();
            throw new Exception();
        }

        var loginInfoResponseContent = await loginInfoResponse.Content.ReadAsStringAsync();
        LogApiCallSuccessful(loginInfoResponseContent);
    }

    private record OigCommand(
        [property: JsonPropertyName("id_device")]
        string DeviceId,
        [property: JsonPropertyName("table")] string Table,
        [property: JsonPropertyName("column")] string Column,
        [property: JsonPropertyName("value")] string Value);

    public void Dispose()
    {
        _client.Dispose();
    }
}
