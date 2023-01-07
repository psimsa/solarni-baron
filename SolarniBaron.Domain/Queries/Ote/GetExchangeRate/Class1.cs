using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Queries.Ote.GetExchangeRate;

public record GetExchangeRateQuery(DateOnly date) : IQuery<GetExchangeRateQueryResponse>;

public record GetExchangeRateQueryResponse(decimal Rate);

public class GetExchangeRateQueryHandler : IQueryHandler<IQuery<GetExchangeRateQueryResponse>, GetExchangeRateQueryResponse>
{
    public Task<GetExchangeRateQueryResponse> Get(IQuery<GetExchangeRateQueryResponse> query)
    {
        throw new NotImplementedException();
    }
}
