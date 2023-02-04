using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

#pragma warning disable CA2007

namespace SolarniBaron.Func;

public partial class OteDay
{
    private readonly ISolarniBaronDispatcher _dispatcher;
    private readonly ILogger _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data")]
    private partial void LogErrorGettingOteData();


    public OteDay(ISolarniBaronDispatcher dispatcher, ILoggerFactory loggerFactory)
    {
        _dispatcher = dispatcher;
        _logger = loggerFactory.CreateLogger<OteDay>();
    }

    [Function("ote/day/{date}")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string date)
    {
        var canParse = DateOnly.TryParse(date, out var parsedDate);
        if (!canParse)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var result = await _dispatcher.Dispatch(new GetPricelistQuery(parsedDate));

        if (result is null)
        {
            LogErrorGettingOteData();
            return req.CreateResponse(HttpStatusCode.NotFound);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(JsonSerializer.Serialize(result, CommonSerializationContext.Default.GetPricelistQueryResponse));
        return response;
    }
}
