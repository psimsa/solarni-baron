using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.BatteryBox.Commands.SetMode;

public record SetModeCommandResponse(ResponseStatus ResponseStatus = ResponseStatus.Ok, string? Error = null)
{
    public static SetModeCommandResponse Empty() =>
        new SetModeCommandResponse(ResponseStatus.Error);
}
