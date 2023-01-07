using System.Text.Json.Serialization;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

namespace SolarniBaron.Domain;

[JsonSerializable(typeof(LoginInfo))]
[JsonSerializable(typeof(BatteryBoxUnits))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(BatteryBoxStatus))]
public partial class CommonSerializationContext : JsonSerializerContext
{
}
