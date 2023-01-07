using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Commands;

namespace SolarniBaron.Func;

public class SetMode
{
    private readonly ICommandHandler<SetModeCommand, SetModeCommandResponse> _commandHandler;
    private readonly ILogger _logger;

    public SetMode(ICommandHandler<SetModeCommand, SetModeCommandResponse> commandHandler, ILoggerFactory loggerFactory)
    {
        _commandHandler = commandHandler;
        _logger = loggerFactory.CreateLogger<SetMode>();
    }

    [Function("batterybox/setmode")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        var setModeInfo = JsonSerializer.Deserialize<SetModeInfo>(req.Body);
        var data = await _commandHandler.Execute(new SetModeCommand(setModeInfo.Email, setModeInfo.Password, setModeInfo.UnitId,
            setModeInfo.Mode));
        if (data.ResponseStatus == ResponseStatus.Error)
        {
            _logger.LogError(data.Error);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await errorResponse.WriteStringAsync(data.Error);
            return errorResponse;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(JsonSerializer.Serialize(data, CommonSerializationContext.Default.SetModeCommandResponse));
        return response;
    }
}
