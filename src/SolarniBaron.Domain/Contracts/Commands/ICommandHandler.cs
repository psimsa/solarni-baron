namespace SolarniBaron.Domain.Contracts.Commands;

public interface ICommandHandler<TCommand, TResponse> where TCommand : class, ICommand<TCommand, TResponse>
{
    Task<TResponse> Execute(ICommand<TCommand, TResponse> command);
}