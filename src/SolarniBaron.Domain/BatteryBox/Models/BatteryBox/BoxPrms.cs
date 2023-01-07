using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

public record BoxPrms
(
    [property: JsonPropertyName("ison")] decimal Ison,
    [property: JsonPropertyName("status")] decimal Status,
    [property: JsonPropertyName("fain")] decimal Fain,
    [property: JsonPropertyName("faout")] decimal Faout,
    [property: JsonPropertyName("fat")] decimal Fat,
    [property: JsonPropertyName("fadc1")] decimal Fadc1,
    [property: JsonPropertyName("fadc2")] decimal Fadc2,
    [property: JsonPropertyName("fv1")] decimal Fv1,
    [property: JsonPropertyName("fa2")] decimal Fa2,
    [property: JsonPropertyName("bypass")] decimal Bypass,
    [property: JsonPropertyName("bypass_m")]
    decimal BypassM,
    [property: JsonPropertyName("cominvert")]
    decimal Cominvert,
    [property: JsonPropertyName("comelmer")]
    decimal Comelmer,
    [property: JsonPropertyName("combatt")]
    decimal Combatt,
    [property: JsonPropertyName("mode")] OperationMode Mode,
    [property: JsonPropertyName("mode1")] decimal Mode1,
    [property: JsonPropertyName("s_stop_ison")]
    decimal SStopIson,
    [property: JsonPropertyName("lcd_brigh")]
    decimal LcdBrigh,
    [property: JsonPropertyName("led_brigh")]
    decimal LedBrigh,
    [property: JsonPropertyName("fan1")] decimal Fan1,
    [property: JsonPropertyName("fan2")] decimal Fan2,
    [property: JsonPropertyName("bat_min")]
    decimal BatMin,
    [property: JsonPropertyName("bat_ac")] decimal BatAc,
    [property: JsonPropertyName("p_fve")] decimal PFve,
    [property: JsonPropertyName("p_bat")] decimal PBat,
    [property: JsonPropertyName("p_grid")] decimal PGrid,
    [property: JsonPropertyName("p_load")] decimal PLoad,
    [property: JsonPropertyName("acouplmt")]
    decimal Acouplmt,
    [property: JsonPropertyName("rqreset")]
    decimal Rqreset,
    [property: JsonPropertyName("sw")] string Sw,
    [property: JsonPropertyName("ip")] string Ip,
    [property: JsonPropertyName("port")] decimal Port,
    [property: JsonPropertyName("periodsave")]
    decimal Periodsave,
    [property: JsonPropertyName("periodcheck")]
    decimal Periodcheck,
    [property: JsonPropertyName("periodquick")]
    decimal Periodquick,
    [property: JsonPropertyName("comexpanz")]
    decimal Comexpanz,
    [property: JsonPropertyName("commpp")] decimal Commpp,
    [property: JsonPropertyName("bat_format")]
    decimal BatFormat,
    [property: JsonPropertyName("scnt")] decimal Scnt,
    [property: JsonPropertyName("enloads")]
    decimal Enloads,
    [property: JsonPropertyName("utc")] decimal Utc,
    [property: JsonPropertyName("sa")] decimal Sa,
    [property: JsonPropertyName("crct")] decimal Crct,
    [property: JsonPropertyName("crcte")] decimal Crcte,
    [property: JsonPropertyName("nb")] decimal Nb,
    [property: JsonPropertyName("re")] decimal Re,
    [property: JsonPropertyName("ra")] decimal Ra,
    [property: JsonPropertyName("comev")] decimal Comev,
    [property: JsonPropertyName("comrtu")] decimal Comrtu
);
