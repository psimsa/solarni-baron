﻿using DotnetDispatcher.Core;
using Microsoft.Extensions.Logging;

namespace SolarniBaron.Domain.BatteryBox.Commands.SetMode;

public class SetModeCommandHandler : ICommandHandler<SetModeCommand, SetModeCommandResponse>
{
    private readonly IBatteryBoxDataConnector _connector;
    private readonly ILogger<SetModeCommandHandler> _logger;

    public SetModeCommandHandler(IBatteryBoxDataConnector connector, ILogger<SetModeCommandHandler> logger)
    {
        _connector = connector;
        _logger = logger;
    }

    public async Task<SetModeCommandResponse> Execute(SetModeCommand command, CancellationToken cancellationToken = default)
    {
        var mode = await _connector.SetMode(command.Username, command.Password, command.UnitId,
            command.Mode);

        return new SetModeCommandResponse(mode.Success, mode.Error);
    }
}
