using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record DcIn
(
    [property: JsonPropertyName("fv_v1")] decimal FvV1,
    [property: JsonPropertyName("fv_i1")] decimal FvI1,
    [property: JsonPropertyName("fv_v2")] decimal FvV2,
    [property: JsonPropertyName("fv_i2")] decimal FvI2,
    [property: JsonPropertyName("fv_p1")] decimal FvP1,
    [property: JsonPropertyName("fv_p2")] decimal FvP2,
    [property: JsonPropertyName("fv_ad")] decimal FvAd,
    [property: JsonPropertyName("fv_am")] decimal FvAm,
    [property: JsonPropertyName("fv_ay")] decimal FvAy,
    [property: JsonPropertyName("fv_proc")]
    decimal FvProc
);