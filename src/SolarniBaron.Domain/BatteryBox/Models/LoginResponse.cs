using System.Text.Json;

namespace SolarniBaron.Domain.BatteryBox.Models;

public record LoginResponse(JsonElement[][] responses);