using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models;

public record LoginResponse([property: JsonPropertyName("responses")]
    JsonElement[][] Responses);
