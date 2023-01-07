using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record CarChargePrms
(
    [property: JsonPropertyName("a_max")] string AMax,
    [property: JsonPropertyName("hdo1")] decimal Hdo1,
    [property: JsonPropertyName("consum_liq")]
    decimal ConsumLiq,
    [property: JsonPropertyName("price_liq")]
    decimal PriceLiq,
    [property: JsonPropertyName("consum_el")]
    decimal ConsumEl,
    [property: JsonPropertyName("price_et1")]
    decimal PriceEt1,
    [property: JsonPropertyName("price_et2")]
    decimal PriceEt2,
    [property: JsonPropertyName("caracusize")]
    decimal Caracusize,
    [property: JsonPropertyName("etocar")] decimal Etocar,
    [property: JsonPropertyName("carcharge")]
    decimal Carcharge,
    [property: JsonPropertyName("ison")] decimal Ison,
    [property: JsonPropertyName("mode")] decimal Mode,
    [property: JsonPropertyName("a_chrg")] string AChrg,
    [property: JsonPropertyName("zone1_s")]
    decimal Zone1S,
    [property: JsonPropertyName("zone1_e")]
    decimal Zone1E,
    [property: JsonPropertyName("zone2_s")]
    decimal Zone2S,
    [property: JsonPropertyName("zone2_e")]
    decimal Zone2E,
    [property: JsonPropertyName("hdo")] decimal Hdo,
    [property: JsonPropertyName("ebatmin")]
    decimal Ebatmin,
    [property: JsonPropertyName("epowmax")]
    decimal Epowmax,
    [property: JsonPropertyName("ebatrec")]
    decimal Ebatrec,
    [property: JsonPropertyName("phss")] decimal Phss,
    [property: JsonPropertyName("eregrep")]
    decimal Eregrep,
    [property: JsonPropertyName("eregsns")]
    decimal Eregsns,
    [property: JsonPropertyName("ident")] string Ident);