namespace SolarniBaron.Domain.Contracts.Commands;

public interface ICommand<TCommand, out TResponse> where TCommand : class
{
    TCommand? Data => this as TCommand;
}
