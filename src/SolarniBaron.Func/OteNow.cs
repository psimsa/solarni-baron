using System.Net;
using System.Text.Json;
using DotnetDispatcher.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Domain;

namespace SolarniBaron.Func
{
    public partial class OteNow
    {
        private readonly IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> _queryHandler;
        private readonly ILogger _logger;

        [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data: {Error}")]
        private partial void LogErrorGettingOteData(string error);


        public OteNow(IQueryHandler<GetPricelistQuery, GetPricelistQueryResponse> queryHandler, ILoggerFactory loggerFactory)
        {
            _queryHandler = queryHandler;
            _logger = loggerFactory.CreateLogger<OteNow>();
        }

        [Function("ote/now")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();
            var result = await _queryHandler.Get(new GetPricelistQuery(DateOnly.FromDateTime(pragueDateTimeNow)));

            if (result.ResponseStatus == ResponseStatus.Error)
            {
                LogErrorGettingOteData(result.Error);
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var toReturn = result.Data?.HourlyRateBreakdown.FirstOrDefault(x => x.HourIndex + 1 == pragueDateTimeNow.Hour);
            if (toReturn == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(
                new SuccessResponse<GetPricelistQueryResponseItem>(toReturn),
                CommonSerializationContext.Default.SuccessResponseGetPricelistQueryResponseItem));
            return response;
        }
    }
}
