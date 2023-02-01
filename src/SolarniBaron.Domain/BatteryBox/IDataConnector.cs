using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

namespace SolarniBaron.Domain.BatteryBox;

public interface IBatteryBoxDataConnector
{
    Task<string> GetRawStats(string username, string password);

    Task<BatteryBoxUnitData> GetStatsForUnit(string username, string password, string? unitId);

    Task<SetModeResponseData> SetMode(string username, string password, string unitId, string mode);
}
