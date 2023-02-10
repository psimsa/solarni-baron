namespace SolarniBaron.Web.Core;

public interface IBackgroundSyncService
{
    Task? Start(CancellationToken cancellationToken);
    Task Stop(CancellationToken cancellationToken);
}
