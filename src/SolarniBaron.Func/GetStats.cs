using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;

namespace SolarniBaron.Func;

public partial class GetStats
{
    private readonly IQueryHandler<GetStatsQuery, GetStatsQueryResponse> _queryHandler;
    private readonly ILogger _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting stats: {Error}")] private partial void LogErrorGettingStats(string error);

    public GetStats(IQueryHandler<GetStatsQuery, GetStatsQueryResponse> queryHandler, ILoggerFactory loggerFactory)
    {
        _queryHandler = queryHandler;
        _logger = loggerFactory.CreateLogger<GetStats>();
    }

    [Function("batterybox/getstats")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        var loginInfo = JsonSerializer.Deserialize<LoginInfo>(req.Body);
        var data = await _queryHandler.Get(new GetStatsQuery(loginInfo.Email, loginInfo.Password, loginInfo.UnitId));
        if (data.ResponseStatus == ResponseStatus.Error)
        {
            LogErrorGettingStats(data.Error);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            errorResponse.WriteString(data.Error);
            return errorResponse;
        }
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        response.WriteString(JsonSerializer.Serialize(data.BatteryBoxStatus));
        return response;
    }
}
