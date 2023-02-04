using DotnetDispatcher.Core;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQuery(string Username, string Password, string? UnitId) : IQuery<GetStatsQueryResponse>;
