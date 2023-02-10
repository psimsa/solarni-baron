using BlazorState;

namespace SolarniBaron.Web.Core;

public interface IActionDispatcherService
{
    Task DispatchAction(IAction action);
}
