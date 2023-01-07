namespace SolarniBaron.Domain.Contracts.Queries;

public interface IQueryHandler<TQuery, TResponse> where TQuery : class, IQuery<TQuery, TResponse>
{
    Task<TResponse> Get(IQuery<TQuery, TResponse> query);
}
