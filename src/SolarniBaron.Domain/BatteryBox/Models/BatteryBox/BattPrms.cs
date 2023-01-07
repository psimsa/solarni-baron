using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

public record BattPrms
(
    [property: JsonPropertyName("bat_n")] decimal BatN,
    [property: JsonPropertyName("bat_ci")] decimal BatCi,
    [property: JsonPropertyName("bat_cu")] decimal BatCu,
    [property: JsonPropertyName("fmt_on")] decimal FmtOn,
    [property: JsonPropertyName("fmt_progress")]
    decimal FmtProgress,
    [property: JsonPropertyName("bat_hdo")]
    decimal BatHdo,
    [property: JsonPropertyName("bat_aa")] decimal BatAa,
    [property: JsonPropertyName("bat_min")]
    decimal BatMin,
    [property: JsonPropertyName("bat_gl_min")]
    decimal BatGlMin,
    [property: JsonPropertyName("bat_ag_min")]
    decimal BatAgMin,
    [property: JsonPropertyName("hdo1_s")] decimal Hdo1S,
    [property: JsonPropertyName("hdo1_e")] decimal Hdo1E,
    [property: JsonPropertyName("hdo2_s")] decimal Hdo2S,
    [property: JsonPropertyName("hdo2_e")] decimal Hdo2E,
    [property: JsonPropertyName("bal_on")] decimal BalOn,
    [property: JsonPropertyName("lo_day")] decimal LoDay,
    [property: JsonPropertyName("bat_di")] decimal BatDi,
    [property: JsonPropertyName("id_subd")]
    decimal IdSubd,
    [property: JsonPropertyName("bat_v1")] decimal BatV1,
    [property: JsonPropertyName("bat_v2")] decimal BatV2,
    [property: JsonPropertyName("bat_n1")] decimal BatN1,
    [property: JsonPropertyName("bat_n2")] decimal BatN2,
    [property: JsonPropertyName("bat_t1")] decimal BatT1,
    [property: JsonPropertyName("bat_t2")] decimal BatT2,
    [property: JsonPropertyName("bat_n3")] decimal BatN3,
    [property: JsonPropertyName("bat_n4")] decimal BatN4
);