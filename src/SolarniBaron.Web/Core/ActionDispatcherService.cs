using BlazorState;
using MediatR;

namespace SolarniBaron.Web.Core;

internal class ActionDispatcherService : IActionDispatcherService
{
    private readonly IMediator _mediator;

    public ActionDispatcherService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAction(IAction action) => await _mediator.Send(action);
}
