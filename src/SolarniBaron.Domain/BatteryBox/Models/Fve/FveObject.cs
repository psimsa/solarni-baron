using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record FveObject
(
    [property: JsonPropertyName("ac_in")] AcIn AcIn,
    [property: JsonPropertyName("ac_in_b")]
    AcInB AcInB,
    [property: JsonPropertyName("ac_out")] AcOut AcOut,
    [property: JsonPropertyName("aircon")] List<object> Aircon,
    [property: JsonPropertyName("aircon_prms")]
    Prms AirconPrms,
    [property: JsonPropertyName("batt")] Batt Batt,
    [property: JsonPropertyName("batt_prms")]
    BattPrms BattPrms,
    [property: JsonPropertyName("boiler")] List<object> Boiler,
    [property: JsonPropertyName("boiler_prms")]
    BoilerPrms BoilerPrms,
    [property: JsonPropertyName("box")] Box Box,
    [property: JsonPropertyName("box_prms")]
    BoxPrms BoxPrms,
    [property: JsonPropertyName("car_charge")]
    List<object> CarCharge,
    [property: JsonPropertyName("car_charge_prms")]
    CarChargePrms CarChargePrms,
    [property: JsonPropertyName("dc_in")] DcIn DcIn,
    [property: JsonPropertyName("device")] Device Device,
    [property: JsonPropertyName("h_pump")] List<object> HPump,
    [property: JsonPropertyName("h_pump_prms")]
    Prms HPumpPrms,
    [property: JsonPropertyName("invertor_prms")]
    InvertorPrms InvertorPrms,
    [property: JsonPropertyName("recuper")]
    List<object> Recuper,
    [property: JsonPropertyName("recuper_prms")]
    Prms RecuperPrms,
    [property: JsonPropertyName("wl_charge")]
    List<object> WlCharge,
    [property: JsonPropertyName("wl_charge_prms")]
    Prms WlChargePrms
);