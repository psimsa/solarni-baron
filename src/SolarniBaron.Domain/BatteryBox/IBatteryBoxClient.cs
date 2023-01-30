namespace SolarniBaron.Domain.BatteryBox;

public interface IBatteryBoxClient : IDisposable
{
    Task<string> GetRawStats(string username, string password);
    Task<(bool, string?)> SetMode(string username, string password, string unitId, string mode);
}
