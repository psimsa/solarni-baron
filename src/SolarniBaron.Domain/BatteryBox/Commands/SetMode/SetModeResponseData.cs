namespace SolarniBaron.Domain.BatteryBox.Commands.SetMode;

public readonly record struct SetModeCommandResponse(bool Success, string? Error);
public readonly record struct SetModeResponseData(bool Success, string? Error);
