using BlazorState;
using MediatR;
using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.BatteryBox.Models.BatteryBox;

namespace SolarniBaron.Web.Core;

public class AppState : State<AppState>
{
    public BatteryBoxStatus BatteryBoxStatus { get; private set; }

    public override void Initialize()
    {
        BatteryBoxStatus = BatteryBoxStatus.Empty();
    }

    public record SetBatteryBoxStatusAction(BatteryBoxStatus newBatteryBoxStatus) : IAction;

    public class SetBatteryBoxStatusHandler : ActionHandler<SetBatteryBoxStatusAction>
    {
        private AppState State => Store.GetState<AppState>();

        public SetBatteryBoxStatusHandler(IStore store) : base(store)
        {
        }

        public override Task<Unit> Handle(SetBatteryBoxStatusAction action, CancellationToken cancellationToken)
        {
            State.BatteryBoxStatus = action.newBatteryBoxStatus;
            return Unit.Task;
        }
    }
}
