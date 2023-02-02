using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Commands;

namespace SolarniBaron.Domain.BatteryBox.Commands.SetMode;

public record SetModeCommandResponse
    (SetModeResponseData Data, ResponseStatus ResponseStatus = ResponseStatus.Ok, string? Error = null) :
        CommandResponse<SetModeResponseData>(Data, ResponseStatus, Error);
