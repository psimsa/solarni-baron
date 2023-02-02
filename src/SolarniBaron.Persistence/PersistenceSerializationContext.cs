using System.Text.Json.Serialization;
using SolarniBaron.Domain.BatteryBox.Models;

namespace SolarniBaron.Persistence;

[JsonSerializable(typeof(LoginInfo))]
public partial class PersistenceSerializationContext : JsonSerializerContext
{
}
