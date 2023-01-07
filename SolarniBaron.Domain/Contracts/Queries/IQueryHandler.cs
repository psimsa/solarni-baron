namespace SolarniBaron.Domain.Contracts;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Get(IQuery<TResponse> query);
}