namespace SolarniBaron.Domain.Contracts;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Execute(ICommand<TResponse> command);
}