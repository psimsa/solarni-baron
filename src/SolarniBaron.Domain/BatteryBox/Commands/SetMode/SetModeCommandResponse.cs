using SolarniBaron.Domain.BatteryBox.Models;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.BatteryBox.Commands.SetMode;

public record SetModeCommandResponse(SetModeResponseData Data, ResponseStatus ResponseStatus = ResponseStatus.Ok, string? Error = null): CommandResponse<SetModeResponseData>(Data, ResponseStatus, Error);
