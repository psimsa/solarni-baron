using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQueryResponse(BatteryBoxStatus BatteryBoxStatus, ResponseStatus ResponseStatus = ResponseStatus.Ok,
    string? Error = null) : QueryResponse<BatteryBoxStatus>(BatteryBoxStatus, ResponseStatus, Error);
