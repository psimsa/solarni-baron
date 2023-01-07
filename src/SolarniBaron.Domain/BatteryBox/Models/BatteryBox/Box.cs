using System.Text.Json.Serialization;

namespace SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

public record Box
(
    [property: JsonPropertyName("temp")] decimal Temp,
    [property: JsonPropertyName("humid")] decimal Humid,
    [property: JsonPropertyName("msc_self")]
    decimal MscSelf,
    [property: JsonPropertyName("strnght")]
    decimal Strnght,
    [property: JsonPropertyName("tmlastcall")]
    decimal Tmlastcall
);