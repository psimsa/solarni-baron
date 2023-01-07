using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record Prms
(
    [property: JsonPropertyName("ison")] decimal Ison,
    [property: JsonPropertyName("prrty")] decimal Prrty,
    [property: JsonPropertyName("comok")] decimal Comok,
    [property: JsonPropertyName("p_set")] decimal PSet,
    [property: JsonPropertyName("addr")] string Addr
);