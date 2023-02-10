using BlazorState;
using MediatR;
using Microsoft.JSInterop;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;
using SolarniBaron.Domain.Ote.Models;

namespace SolarniBaron.Web.Core;

public class AppState : State<AppState>
{
    public BatteryBoxStatus BatteryBoxStatus { get; private set; }
    public bool IsBackgroundSyncing { get; private set; }
    public bool ShouldDisplayLoginBar { get; private set; }
    public IReadOnlyCollection<PriceListItem>? PriceOutlook { get; private set; }

    public override void Initialize()
    {
        BatteryBoxStatus = BatteryBoxStatus.Empty();
        PriceOutlook = Array.Empty<PriceListItem>();
    }

    public record SetBatteryBoxStatusAction(BatteryBoxStatus NewBatteryBoxStatus) : IAction;
    public record SetIsBackgroundSyncingAction(bool NewIsBackgroundSyncing) : IAction;
    public record SetShouldDisplayLoginBarAction(bool NewShouldDisplayLoginBar) : IAction;
    public record SetPriceOutlookAction(IReadOnlyCollection<PriceListItem>? NewPriceOutlook) : IAction;

    public class SetBatteryBoxStatusHandler : ActionHandler<SetBatteryBoxStatusAction>
    {
        private AppState State => Store.GetState<AppState>();

        public SetBatteryBoxStatusHandler(IStore store) : base(store)
        {
        }

        public override Task<Unit> Handle(SetBatteryBoxStatusAction action, CancellationToken cancellationToken)
        {
            State.BatteryBoxStatus = action.NewBatteryBoxStatus;
            return Unit.Task;
        }
    }

    public class SetIsBackgroundSyncingHandler : ActionHandler<SetIsBackgroundSyncingAction>
    {
        private AppState State => Store.GetState<AppState>();

        public SetIsBackgroundSyncingHandler(IStore store) : base(store)
        {
        }

        public override Task<Unit> Handle(SetIsBackgroundSyncingAction action, CancellationToken cancellationToken)
        {
            State.IsBackgroundSyncing = action.NewIsBackgroundSyncing;
            return Unit.Task;
        }
    }

    public class SetShouldDisplayLoginBarHandler : ActionHandler<SetShouldDisplayLoginBarAction>
    {
        private AppState State => Store.GetState<AppState>();

        public SetShouldDisplayLoginBarHandler(IStore store) : base(store)
        {
        }

        public override Task<Unit> Handle(SetShouldDisplayLoginBarAction action, CancellationToken cancellationToken)
        {
            State.ShouldDisplayLoginBar = action.NewShouldDisplayLoginBar;
            return Unit.Task;
        }
    }

    public class SetPriceOutlookHandler : ActionHandler<SetPriceOutlookAction>
    {
        private readonly IJSRuntime _runtime;
        private AppState State => Store.GetState<AppState>();

        public SetPriceOutlookHandler(IStore store, IJSRuntime runtime) : base(store)
        {
            _runtime = runtime;
        }

        public override async Task<Unit> Handle(SetPriceOutlookAction action, CancellationToken cancellationToken)
        {
            State.PriceOutlook = action.NewPriceOutlook;
            return Unit.Value;
        }
    }
}
