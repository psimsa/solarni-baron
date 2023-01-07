using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

public record AcOut
(
    [property: JsonPropertyName("aco_vr")] decimal AcoVr,
    [property: JsonPropertyName("aco_vs")] decimal AcoVs,
    [property: JsonPropertyName("aco_vt")] decimal AcoVt,
    [property: JsonPropertyName("aco_pr")] decimal AcoPr,
    [property: JsonPropertyName("aco_ps")] decimal AcoPs,
    [property: JsonPropertyName("aco_pt")] decimal AcoPt,
    [property: JsonPropertyName("aco_p")] decimal AcoP,
    [property: JsonPropertyName("aco_par")]
    decimal AcoPar,
    [property: JsonPropertyName("aco_pas")]
    decimal AcoPas,
    [property: JsonPropertyName("aco_pat")]
    decimal AcoPat,
    [property: JsonPropertyName("aco_pa")] decimal AcoPa,
    [property: JsonPropertyName("aco_f")] decimal AcoF,
    [property: JsonPropertyName("en_day")] decimal EnDay, // dnes spotrebovano
    [property: JsonPropertyName("en_mont")]
    decimal EnMont,
    [property: JsonPropertyName("en_year")]
    decimal EnYear
);
