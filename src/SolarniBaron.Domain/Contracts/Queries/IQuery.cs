namespace SolarniBaron.Domain.Contracts;

public interface IQuery<TQuery, out TResponse> where TQuery : class
{
    TQuery? Query => this as TQuery;
}