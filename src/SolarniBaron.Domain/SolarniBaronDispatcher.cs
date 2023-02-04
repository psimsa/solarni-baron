using DotnetDispatcher.Attributes;
using DotnetDispatcher.Core;
using SolarniBaron.Domain.BatteryBox.Commands.SetMode;
using SolarniBaron.Domain.BatteryBox.Queries.GetStats;
using SolarniBaron.Domain.CNB.Queries.GetExchangeRate;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;
using SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

namespace SolarniBaron.Domain;

[GenerateDispatcher(typeof(GetPriceOutlookQuery))]
[GenerateDispatcher(typeof(GetPricelistQuery))]
[GenerateDispatcher(typeof(SetModeCommand))]
[GenerateDispatcher(typeof(GetStatsQuery))]
[GenerateDispatcher(typeof(GetExchangeRateQuery))]
public partial class SolarniBaronDispatcher : DispatcherBase
{
    public SolarniBaronDispatcher(IServiceProvider serviceProvider) : base(serviceProvider)
    {       
    }
}
