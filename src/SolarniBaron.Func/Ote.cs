using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
#pragma warning disable CA2007

namespace SolarniBaron.Func;

public class Ote
{
    private readonly IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> _queryHandler;
    private readonly ILogger _logger;

    public Ote(IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> queryHandler, ILoggerFactory loggerFactory)
    {
        _queryHandler = queryHandler;
        _logger = loggerFactory.CreateLogger<Ote>();
    }

    [Function("ote/{date}")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, DateOnly? date)
    {
        var result = await _queryHandler.Get(new GetPricelistQuery(date ?? DateOnly.FromDateTime(DateTime.Now)));

        if (result.ResponseStatus == ResponseStatus.Error)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.NotFound);
            return errorResponse;
        }
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(JsonSerializer.Serialize(result));
        return response;
    }
}
