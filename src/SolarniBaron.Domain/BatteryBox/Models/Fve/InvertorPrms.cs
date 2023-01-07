using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record InvertorPrms
(
    [property: JsonPropertyName("err_pv")] decimal ErrPv,
    [property: JsonPropertyName("err_batt")]
    decimal ErrBatt,
    [property: JsonPropertyName("err_grid")]
    decimal ErrGrid,
    [property: JsonPropertyName("err_ac")] decimal ErrAc,
    [property: JsonPropertyName("err_else")]
    decimal ErrElse,
    [property: JsonPropertyName("dbg1")] decimal Dbg1,
    [property: JsonPropertyName("dbg2")] decimal Dbg2,
    [property: JsonPropertyName("dbg3")] decimal Dbg3,
    [property: JsonPropertyName("dbg4")] decimal Dbg4,
    [property: JsonPropertyName("t_inn")] decimal TInn,
    [property: JsonPropertyName("t_inn1")] decimal TInn1,
    [property: JsonPropertyName("prrty")] decimal Prrty,
    [property: JsonPropertyName("charge")] decimal Charge,
    [property: JsonPropertyName("charge_ac")]
    decimal ChargeAc,
    [property: JsonPropertyName("to_grid")]
    decimal ToGrid,
    [property: JsonPropertyName("load_pv_on")]
    decimal LoadPvOn,
    [property: JsonPropertyName("load_pv_off")]
    decimal LoadPvOff,
    [property: JsonPropertyName("grid_pv_on")]
    decimal GridPvOn,
    [property: JsonPropertyName("grid_pv_off")]
    decimal GridPvOff,
    [property: JsonPropertyName("mode")] decimal Mode,
    [property: JsonPropertyName("model")] decimal Model,
    [property: JsonPropertyName("prll_out")]
    decimal PrllOut,
    [property: JsonPropertyName("p_adj_strt")]
    decimal PAdjStrt,
    [property: JsonPropertyName("p_adj_enbl")]
    decimal PAdjEnbl,
    [property: JsonPropertyName("pf_min_100")]
    decimal PfMin100
);