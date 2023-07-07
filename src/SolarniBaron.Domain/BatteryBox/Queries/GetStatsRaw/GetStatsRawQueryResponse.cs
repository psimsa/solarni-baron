using System.Text.Json.Nodes;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStatsRaw;

public record GetStatsRawQueryResponse(Dictionary<string, JsonNode> BatteryBoxUnitData);
