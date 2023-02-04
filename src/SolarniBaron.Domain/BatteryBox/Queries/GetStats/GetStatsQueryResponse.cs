using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQueryResponse(BatteryBoxStatus BatteryBoxStatus);
