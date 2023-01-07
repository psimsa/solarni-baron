using SolarniBaron.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarniBaron.Domain.BatteryBox.Queries.GetStats;

public record GetStatsQuery(string Username, string Password, string? UnitId) : IQuery<GetStatsQuery, GetStatsQueryResponse>;
