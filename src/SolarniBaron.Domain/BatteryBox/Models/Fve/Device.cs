using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record Device
(
    [property: JsonPropertyName("id_lastset")]
    decimal IdLastset,
    [property: JsonPropertyName("id_type")]
    decimal IdType,
    [property: JsonPropertyName("id_location")]
    decimal IdLocation,
    [property: JsonPropertyName("lastip")] string Lastip,
    [property: JsonPropertyName("latdat")] decimal Latdat,
    [property: JsonPropertyName("latreq")] decimal Latreq,
    [property: JsonPropertyName("lastcall")]
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Lastcall,
    [property: JsonPropertyName("lastupdate")]
    string Lastupdate,
    [property: JsonPropertyName("lastset")]
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Lastset,
    [property: JsonPropertyName("lastweather")]
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Lastweather,
    [property: JsonPropertyName("userlaston")]
    string Userlaston,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("fw_base")]
    string FwBase,
    [property: JsonPropertyName("fw_lastversion")]
    string FwLastversion,
    [property: JsonPropertyName("fw_lastfile")]
    string FwLastfile,
    [property: JsonPropertyName("fw_updateenabled")]
    string FwUpdateenabled,
    [property: JsonPropertyName("resetenabled")]
    string Resetenabled,
    [property: JsonPropertyName("testing")]
    string Testing,
    [property: JsonPropertyName("settingenabled")]
    string Settingenabled,
    [property: JsonPropertyName("readoutenabled")]
    string Readoutenabled,
    [property: JsonPropertyName("weatherenabled")]
    string Weatherenabled,
    [property: JsonPropertyName("timeenabled")]
    string Timeenabled,
    [property: JsonPropertyName("hcrequired")]
    string Hcrequired,
    [property: JsonPropertyName("record")] string Record,
    [property: JsonPropertyName("id_server")]
    decimal IdServer
);
