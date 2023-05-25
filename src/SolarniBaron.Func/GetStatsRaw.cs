﻿using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SolarniBaron.Domain;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.BatteryBox.Queries.GetStatsRaw;

namespace SolarniBaron.Func
{
    public partial class GetStatsRaw
    {
        private readonly ISolarniBaronDispatcher _dispatcher;
        private readonly ILogger _logger;

        [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error getting stats")]
        private partial void LogErrorGettingStats();

        public GetStatsRaw(ISolarniBaronDispatcher dispatcher, ILoggerFactory loggerFactory)
        {
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger<GetStatsRaw>();
        }

        [Function("batterybox/getstatsraw")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var loginInfo = JsonSerializer.Deserialize<LoginInfo>(req.Body);
            var data = await _dispatcher.Dispatch(new GetStatsRawQuery(loginInfo.Email, loginInfo.Password, loginInfo.UnitId));
            if (data is null)
            {
                LogErrorGettingStats();
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                errorResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                return errorResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonSerializer.Serialize(data.BatteryBoxUnitData));
            return response;
        }
    }
}
