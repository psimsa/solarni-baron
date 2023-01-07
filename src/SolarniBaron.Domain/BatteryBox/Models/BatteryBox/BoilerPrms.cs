using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

public record BoilerPrms
(
    [property: JsonPropertyName("ison")] decimal Ison,
    [property: JsonPropertyName("prrty")] decimal Prrty,
    [property: JsonPropertyName("p_set")] decimal PSet,
    [property: JsonPropertyName("zone1_s")]
    decimal Zone1S,
    [property: JsonPropertyName("zone1_e")]
    decimal Zone1E,
    [property: JsonPropertyName("zone2_s")]
    decimal Zone2S,
    [property: JsonPropertyName("zone2_e")]
    decimal Zone2E,
    [property: JsonPropertyName("zone3_s")]
    decimal Zone3S,
    [property: JsonPropertyName("zone3_e")]
    decimal Zone3E,
    [property: JsonPropertyName("zone4_s")]
    decimal Zone4S,
    [property: JsonPropertyName("zone4_e")]
    decimal Zone4E,
    [property: JsonPropertyName("hdo")] decimal Hdo,
    [property: JsonPropertyName("termostat")]
    decimal Termostat,
    [property: JsonPropertyName("manual")] decimal Manual,
    [property: JsonPropertyName("offset")] decimal Offset,
    [property: JsonPropertyName("wd")] decimal Wd,
    [property: JsonPropertyName("ssr0")] decimal Ssr0,
    [property: JsonPropertyName("ssr1")] decimal Ssr1,
    [property: JsonPropertyName("ssr2")] decimal Ssr2
);