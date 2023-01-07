using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record Batt
(
    [property: JsonPropertyName("bat_v")] decimal BatV,
    [property: JsonPropertyName("bat_c")] decimal BatC,
    [property: JsonPropertyName("bat_i")] decimal BatI,
    [property: JsonPropertyName("bat_t")] decimal BatT,
    [property: JsonPropertyName("bat_q")] decimal BatQ,
    [property: JsonPropertyName("bat_ad")] decimal BatAd,
    [property: JsonPropertyName("bat_am")] decimal BatAm,
    [property: JsonPropertyName("bat_ay")] decimal BatAy,
    [property: JsonPropertyName("bat_p")] decimal BatP,
    [property: JsonPropertyName("bat_apd")]
    decimal BatApd,
    [property: JsonPropertyName("bat_and")]
    decimal BatAnd
);