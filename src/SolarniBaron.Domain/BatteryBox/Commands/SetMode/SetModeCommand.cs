using SolarniBaron.Domain.Contracts.Commands;

namespace SolarniBaron.Domain.BatteryBox.Commands.SetMode;

public record SetModeCommand
    (string Username, string Password, string UnitId, string Mode) : ICommand<SetModeCommand, SetModeCommandResponse>;
