using SolarniBaron.Domain.BatteryBox.Models;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQueryResponse(BatteryBoxStatus BatteryBoxStatus);
