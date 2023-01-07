namespace SolarniBaron.Domain.Contracts.Queries;

public interface IQuery<TQuery, out TResponse> where TQuery : class
{
    TQuery? Data => this as TQuery;
}
