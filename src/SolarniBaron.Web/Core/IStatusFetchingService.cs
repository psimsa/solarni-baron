namespace SolarniBaron.Web.Core;

public interface IStatusFetchingService
{
    Task Start(CancellationToken cancellationToken);
    Task Stop(CancellationToken cancellationToken);
}