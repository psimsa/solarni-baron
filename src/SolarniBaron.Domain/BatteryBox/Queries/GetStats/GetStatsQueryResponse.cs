using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQueryResponse(BatteryBoxStatus BatteryBoxStatus, ResponseStatus ResponseStatus = ResponseStatus.Ok,
    string? Error = null)
{
    public static GetStatsQueryResponse Empty() =>
        new GetStatsQueryResponse(BatteryBoxStatus.Empty(), ResponseStatus.Error);
}
