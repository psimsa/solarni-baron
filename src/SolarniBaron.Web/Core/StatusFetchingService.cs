using System.Net.Http.Json;
using System.Threading;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain;

namespace SolarniBaron.Web.Core;

public class StatusFetchingService : IStatusFetchingService, IDisposable
{
    private readonly IActionDispatcherService _actionDispatcherService;
    private readonly ClientConfig _config;
    private readonly HttpClient _client;
    private readonly IStorage _storage;
    private readonly ILogger<StatusFetchingService> _logger;
    private CancellationToken _token;
    private Task _task;
    private CancellationTokenSource _cts;
    private LoginInfo _loginInfo;

    public StatusFetchingService(IActionDispatcherService actionDispatcherService, ClientConfig config, HttpClient client, IStorage storage, ILogger<StatusFetchingService> logger)
    {
        _actionDispatcherService = actionDispatcherService;
        _config = config;
        _client = client;
        _storage = storage;
        _logger = logger;
    }

    public Task Start(CancellationToken cancellationToken)
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

    private async Task FetchStatus(CancellationToken token)
    {
        _logger.LogDebug("Starting background worker");
        
        var urls = new[] {_config.LocalGetStatsUrl, "/api/batterybox/getstats"};
        var i = 0;
        
        while (!token.IsCancellationRequested)
        {
            try
            {
                await _actionDispatcherService.DispatchAction(new AppState.SetIsBackgroundSyncingAction(true));
                _loginInfo = await _storage.GetItem<LoginInfo>("loginInfo") ?? LoginInfo.Empty;
                _logger.LogDebug("Starting background sync");
                var response = await _client.PostAsync(urls[i], JsonContent.Create(_loginInfo), token);
                response.EnsureSuccessStatusCode();

                var messageResponse = await response.Content.ReadFromJsonAsync(CommonSerializationContext.Default.BatteryBoxStatus, token);
                if (messageResponse != null)
                {
                    await _actionDispatcherService.DispatchAction(new AppState.SetBatteryBoxStatusAction(messageResponse));
                    
                    await _storage.SetItem("status", messageResponse);

                    if (string.IsNullOrWhiteSpace(_loginInfo.UnitId))
                    {
                        _loginInfo = _loginInfo with { UnitId = messageResponse.UnitId };
                        await _storage.SetItem("loginInfo", _loginInfo);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while fetching status");
                
                await _actionDispatcherService.DispatchAction(new AppState.SetShouldDisplayLoginBarAction(true));
                await _actionDispatcherService.DispatchAction(new AppState.SetBatteryBoxStatusAction(BatteryBoxStatus.Empty()));
                i++;
                if (i >= urls.Length)
                {
                    i = 0;
                }
                else
                {
                    continue;
                }
            }
            finally
            {
                await _actionDispatcherService.DispatchAction(new AppState.SetIsBackgroundSyncingAction(false));
            }
            await Task.Delay(10_000, token);
        }
        _logger.LogDebug("Stopping background worker");
    }
}
