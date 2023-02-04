using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

#pragma warning disable CA2007

namespace SolarniBaron.Func;

public partial class OteOutlookNow
{
    private readonly ISolarniBaronDispatcher _dispatcher;
    private readonly ILogger _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data")]
    private partial void LogErrorGettingOteData();


    public OteOutlookNow(ISolarniBaronDispatcher dispatcher, ILoggerFactory loggerFactory)
    {
        _dispatcher = dispatcher;
        _logger = loggerFactory.CreateLogger<OteOutlookNow>();
    }

    [Function("ote/outlook/now")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();

        var result = await _dispatcher.Dispatch(new GetPriceOutlookQuery(pragueDateTimeNow));

        if (result is null)
        {
            LogErrorGettingOteData();
            return req.CreateResponse(HttpStatusCode.NotFound);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(
            JsonSerializer.Serialize(result.Data?.HourlyRateBreakdown?.FirstOrDefault(x => x.HourIndex == pragueDateTimeNow.Hour)));
        return response;
    }
}
