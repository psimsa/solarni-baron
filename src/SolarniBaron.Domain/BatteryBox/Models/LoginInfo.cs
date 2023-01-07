using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models;

public record LoginInfo([property: JsonPropertyName("email")] string Email, [property: JsonPropertyName("password")]
    string Password, [property: JsonPropertyName("unitId")] string? UnitId = null);