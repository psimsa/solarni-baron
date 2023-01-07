using System.Text.Json.Serialization;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Models.Fve;

namespace SolarniBaron.Domain;

[JsonSerializable(typeof(LoginInfo))]
[JsonSerializable(typeof(FveResult))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(FveStatus))]
public partial class CommonSerializationContext : JsonSerializerContext
{
}
