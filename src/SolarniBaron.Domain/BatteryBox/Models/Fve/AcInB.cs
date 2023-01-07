using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record AcInB([property: JsonPropertyName("aci_wr")] decimal AciWr,
    [property: JsonPropertyName("aci_ws")] decimal AciWs,
    [property: JsonPropertyName("aci_wt")] decimal AciWt,
    [property: JsonPropertyName("ac_ad")] decimal AcAd,
    [property: JsonPropertyName("ac_am")] decimal AcAm,
    [property: JsonPropertyName("ac_ay")] decimal AcAy
);