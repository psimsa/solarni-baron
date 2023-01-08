using SolarniBaron.Domain.BatteryBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolarniBaron.Persistence;

[JsonSerializable(typeof(LoginInfo))]
public partial class PersistenceSerializationContext : JsonSerializerContext { }
