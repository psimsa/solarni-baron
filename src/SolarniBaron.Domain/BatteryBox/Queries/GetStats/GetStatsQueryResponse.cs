using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQueryResponse(FveStatus FveStatus, ResponseStatus ResponseStatus = ResponseStatus.Ok,
    string? Error = null)
{
    public static GetStatsQueryResponse Empty() =>
        new GetStatsQueryResponse(FveStatus.Empty(), ResponseStatus.Error);
}
