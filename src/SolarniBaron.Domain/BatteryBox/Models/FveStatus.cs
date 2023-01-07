using System.Text.Json.Serialization;
using SolarniBaron.Domain.BatteryBox.Models.Fve;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record FveStatus(
    [property: JsonPropertyName("unitId")] string UnitId,
    [property: JsonPropertyName("panelsOutputL1")]
    decimal PanelsOutputL1,
    [property: JsonPropertyName("panelsOutputL2")]
    decimal PanelsOutputL2,
    [property: JsonPropertyName("panelsOutputPct")]
    decimal PanelsOutputPct,
    [property: JsonPropertyName("batteryPct")]
    decimal BatteryPct,
    [property: JsonPropertyName("consumptionL1")]
    decimal ConsumptionL1,
    [property: JsonPropertyName("consumptionL2")]
    decimal ConsumptionL2,
    [property: JsonPropertyName("consumptionL3")]
    decimal ConsumptionL3,
    [property: JsonPropertyName("consumptionTotal")]
    decimal ConsumptionTotal,
    [property: JsonPropertyName("gridOutputL1")]
    decimal GridOutputL1,
    [property: JsonPropertyName("gridOutputL2")]
    decimal GridOutputL2,
    [property: JsonPropertyName("gridOutputL3")]
    decimal GridOutputL3,
    [property: JsonPropertyName("gridOutputTotal")]
    decimal GridOutputTotal,
    [property: JsonPropertyName("outputToday")]
    decimal OutputToday,
    [property: JsonPropertyName("consumptionToday")]
    decimal ConsumptionToday,
    [property: JsonPropertyName("gridOutputToday")]
    decimal GridOutput,
    [property: JsonPropertyName("lastCall")]
    DateTimeOffset LastCall,
    [property: JsonPropertyName("fveMode")]
    FveMode FveMode = FveMode.Home1)

{
    public static FveStatus FromFveObject(FveObject fve)
    {
        return new FveStatus(
            "",
            fve.DcIn.FvP1,
            fve.DcIn.FvP2,
            fve.DcIn.FvProc,
            fve.Batt.BatC,
            fve.AcOut.AcoPr,
            fve.AcOut.AcoPs,
            fve.AcOut.AcoPt,
            fve.AcOut.AcoP,
            fve.AcIn.AciWr,
            fve.AcIn.AciWs,
            fve.AcIn.AciWt,
            fve.AcIn.AciWr + fve.AcIn.AciWs + fve.AcIn.AciWt,
            fve.DcIn.FvAd,
            fve.AcOut.EnDay,
            fve.AcIn.AcAd,
            fve.Device.Lastcall,
            fve.BoxPrms.Mode
        );
    }

    public static FveStatus Empty() =>
        new FveStatus("",
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            DateTime.UnixEpoch);
}