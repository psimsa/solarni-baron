using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

namespace SolarniBaron.Web.Core;

public class BackgroundSyncService : IBackgroundSyncService, IDisposable
{
    private readonly IActionDispatcherService _actionDispatcherService;
    private readonly ClientConfig _config;
    private readonly HttpClient _client;
    private readonly IStorage _storage;
    private readonly ILogger<BackgroundSyncService> _logger;
    private CancellationToken _token;
    private Task? _task;
    private CancellationTokenSource? _cts;
    private LoginInfo? _loginInfo;
    private DateTime _lastSync = DateTime.MinValue;

    private const string GetStatsUrl = "api/batterybox/getstats";
    private const string OteOutlookUrl = "api/ote/outlook";

    public BackgroundSyncService(IActionDispatcherService actionDispatcherService, ClientConfig config, HttpClient client, IStorage storage,
        ILogger<BackgroundSyncService> logger)
    {
        _actionDispatcherService = actionDispatcherService;
        _config = config;
        _client = client;
        _storage = storage;
        _logger = logger;
    }

    public Task? Start(CancellationToken cancellationToken)
    {
        _cts = new CancellationTokenSource();
        cancellationToken.Register(() => _cts.Cancel());
        _token = _cts.Token;

        _task = FetchStatus(_token);
        return _task;
    }

    public async Task Stop(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        await _task;
    }

    public void Dispose()
    {
        _cts.Dispose();
        _task.Dispose();
    }

    private async Task? FetchStatus(CancellationToken token)
    {
        _logger.LogDebug("Starting background worker");

        var urls = new[] {_config.LocalGetStatsUrl, "/"};
        var i = 0;

        while (!token.IsCancellationRequested)
        {
            try
            {
                await _actionDispatcherService.DispatchAction(new AppState.SetIsBackgroundSyncingAction(true));
                _loginInfo = await _storage.GetItem<LoginInfo>("loginInfo") ?? LoginInfo.Empty;
                _logger.LogDebug("Starting background sync");
                await Task.WhenAll(RefreshStats(token, urls, i), RefreshOte(token, urls, i));
                _lastSync = DateTime.Now;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while fetching status");

                i++;
                if (i >= urls.Length)
                {
                    i = 0;
                }
                else
                {
                    continue;
                }

                await _actionDispatcherService.DispatchAction(new AppState.SetShouldDisplayLoginBarAction(true));
                await _actionDispatcherService.DispatchAction(new AppState.SetBatteryBoxStatusAction(BatteryBoxStatus.Empty()));
            }
            finally
            {
                await _actionDispatcherService.DispatchAction(new AppState.SetIsBackgroundSyncingAction(false));
            }

            await Task.Delay(10_000, token);
        }

        _logger.LogDebug("Stopping background worker");
    }

    private async Task RefreshStats(CancellationToken token, string[] urls, int i)
    {
        var response = await _client.PostAsync($"{urls[i]}{GetStatsUrl}", JsonContent.Create(_loginInfo), token);
        response.EnsureSuccessStatusCode();
        await _actionDispatcherService.DispatchAction(new AppState.SetShouldDisplayLoginBarAction(false));

        var messageResponse =
            await response.Content.ReadFromJsonAsync(CommonSerializationContext.Default.BatteryBoxStatus, token);
        if (messageResponse != null)
        {
            await _actionDispatcherService.DispatchAction(new AppState.SetBatteryBoxStatusAction(messageResponse));

            await _storage.SetItem("status", messageResponse);

            if (string.IsNullOrWhiteSpace(_loginInfo.UnitId))
            {
                _loginInfo = _loginInfo with {UnitId = messageResponse.UnitId};
                await _storage.SetItem("loginInfo", _loginInfo);
            }
        }
    }

    private async Task RefreshOte(CancellationToken token, string[] urls, int i)
    {
        if (DateTime.Now - _lastSync < TimeSpan.FromMinutes(10) && DateTime.Now.Hour == _lastSync.Hour)
        {
            return;
        }

        var response = await _client.GetAsync($"{urls[i]}{OteOutlookUrl}", token);
        response.EnsureSuccessStatusCode();

        var messageResponse =
            await response.Content.ReadFromJsonAsync<GetPriceOutlookQueryResponse>(
                new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
        if (messageResponse != null)
        {
            var messageResponseHourlyRateBreakdown = messageResponse.HourlyRateBreakdown.ToArray();
            // messageResponseHourlyRateBreakdown[3] = messageResponseHourlyRateBreakdown[3] with {BasePriceEur = new Random().Next(0, 100)};
            await _actionDispatcherService.DispatchAction(new AppState.SetPriceOutlookAction(messageResponseHourlyRateBreakdown));

            await _storage.SetItem("ote", messageResponse);
        }
    }
}
