﻿using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Domain;

namespace SolarniBaron.Func
{
    public partial class OteNow
    {
        private readonly ISolarniBaronDispatcher _dispatcher;
        private readonly CommonSerializationContext _serializationContext;
        private readonly ILogger _logger;

        [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting OTE data")]
        private partial void LogErrorGettingOteData();


        public OteNow(ISolarniBaronDispatcher dispatcher, CommonSerializationContext serializationContext, ILoggerFactory loggerFactory)
        {
            _dispatcher = dispatcher;
            _serializationContext = serializationContext;
            _logger = loggerFactory.CreateLogger<OteNow>();
        }

        [Function("ote/now")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var pragueDateTimeNow = DateTimeHelpers.GetPragueDateTimeNow();
            var result = await _dispatcher.Dispatch(new GetPricelistQuery(DateOnly.FromDateTime(pragueDateTimeNow.DateTime)));

            if (result is null)
            {
                LogErrorGettingOteData();
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var toReturn = result.HourlyRateBreakdown?.FirstOrDefault(x => x.HourIndex == pragueDateTimeNow.Hour);
            if (toReturn == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(
                toReturn,
                _serializationContext.PriceListItem));
            return response;
        }
    }
}
