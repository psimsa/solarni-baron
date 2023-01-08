using SolarniBaron.Domain.Contracts.Queries;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQuery(string Username, string Password, string? UnitId) : IQuery<GetStatsQuery, GetStatsQueryResponse>;
