namespace SolarniBaron.Domain.Contracts;

public interface IQuery<out TResponse>
{
}

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Get(IQuery<TResponse> query);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Execute(ICommand<TResponse> command);
}

public interface ICommand<out TResponse>
{
}

public enum ResponseStatus
{
    Ok,
    Error
}

public record Response<TResponse>(TResponse Data, ResponseStatus Status, string Message);