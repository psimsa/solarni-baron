using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models;

public record LoginInfo([property: JsonPropertyName("email")] string Email, [property: JsonPropertyName("password")]
    string Password, [property: JsonPropertyName("unitId")] string? UnitId = null);

public record SetModeInfo([property: JsonPropertyName("email")] string Email, [property: JsonPropertyName("password")]
    string Password, [property: JsonPropertyName("unitId")] string UnitId,
    [property: JsonPropertyName("mode")] string Mode);
