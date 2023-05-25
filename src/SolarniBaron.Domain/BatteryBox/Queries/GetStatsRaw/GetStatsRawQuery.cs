using DotnetDispatcher.Core;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStatsRaw;

public record GetStatsRawQuery(string Username, string Password, string? UnitId) : IQuery<GetStatsRawQueryResponse>;
