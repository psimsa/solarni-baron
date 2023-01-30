using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models;

public record SetModeInfo([property: JsonPropertyName("email")] string Email, [property: JsonPropertyName("password")]
    string Password, [property: JsonPropertyName("unitId")] string UnitId,
    [property: JsonPropertyName("mode")] string Mode);
