using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
#pragma warning disable CA2007

namespace SolarniBaron.Func;

public partial class Ote
{
    private readonly IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> _queryHandler;
    private readonly ILogger _logger;

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data: {Error}")] private partial void LogErrorGettingOteData(string error);


    public Ote(IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> queryHandler, ILoggerFactory loggerFactory)
    {
        _queryHandler = queryHandler;
        _logger = loggerFactory.CreateLogger<Ote>();
    }

    [Function("ote/{date}")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, string date)
    {
        var canParse = DateOnly.TryParse(date, out var parsedDate);
        if (!canParse)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var result = await _queryHandler.Get(new GetPricelistQuery(parsedDate));

        if (result.ResponseStatus == ResponseStatus.Error)
        {
            LogErrorGettingOteData(result.Error);
            return req.CreateResponse(HttpStatusCode.NotFound);
        }
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(JsonSerializer.Serialize(result, CommonSerializationContext.Default.GetPricelistQueryResponse));
        return response;
    }
}
