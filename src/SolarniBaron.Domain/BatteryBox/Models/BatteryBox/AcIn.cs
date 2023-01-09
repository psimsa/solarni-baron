using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

public record AcIn
(
    [property: JsonPropertyName("aci_vr")] decimal AciVr,
    [property: JsonPropertyName("aci_vs")] decimal AciVs,
    [property: JsonPropertyName("aci_vt")] decimal AciVt,
    [property: JsonPropertyName("aci_wr")] decimal AciWr,
    [property: JsonPropertyName("aci_ws")] decimal AciWs,
    [property: JsonPropertyName("aci_wt")] decimal AciWt,
    [property: JsonPropertyName("aci_f")] decimal AciF,
    [property: JsonPropertyName("ac_ad")] decimal AcAd,
    [property: JsonPropertyName("ac_am")] decimal AcAm,
    [property: JsonPropertyName("ac_ay")] decimal AcAy,
    [property: JsonPropertyName("ac_pd")] decimal AcPd,
    [property: JsonPropertyName("ac_pm")] decimal AcPm,
    [property: JsonPropertyName("ac_py")] decimal AcPy
);
