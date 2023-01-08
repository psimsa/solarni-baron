using System.Text.Json.Serialization;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

namespace SolarniBaron.Domain.BatteryBox.Models;

public record BatteryBoxStatus(
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
    [property: JsonPropertyName("operationMode")]
    OperationMode OperationMode = OperationMode.Home1)

{
    public static BatteryBoxStatus FromBatteryBoxUnitData(BatteryBoxUnitData bbUnitData)
    {
        return new BatteryBoxStatus(
            "",
            bbUnitData.DcIn.FvP1,
            bbUnitData.DcIn.FvP2,
            bbUnitData.DcIn.FvProc,
            bbUnitData.Batt.BatC,
            bbUnitData.AcOut.AcoPr,
            bbUnitData.AcOut.AcoPs,
            bbUnitData.AcOut.AcoPt,
            bbUnitData.AcOut.AcoP,
            bbUnitData.AcIn.AciWr,
            bbUnitData.AcIn.AciWs,
            bbUnitData.AcIn.AciWt,
            bbUnitData.AcIn.AciWr + bbUnitData.AcIn.AciWs + bbUnitData.AcIn.AciWt,
            bbUnitData.DcIn.FvAd,
            bbUnitData.AcOut.EnDay,
            bbUnitData.AcIn.AcAd,
            bbUnitData.Device.Lastcall,
            bbUnitData.BoxPrms.Mode
        );
    }

    public static BatteryBoxStatus Empty() =>
        new BatteryBoxStatus("",
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