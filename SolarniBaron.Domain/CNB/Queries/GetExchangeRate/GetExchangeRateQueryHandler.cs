using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.CNB.Queries.GetExchangeRate;

public class GetExchangeRateQueryHandler : IQueryHandler<IQuery<GetExchangeRateQueryResponse>, GetExchangeRateQueryResponse>
{
    public Task<GetExchangeRateQueryResponse> Get(IQuery<GetExchangeRateQueryResponse> query)
    {
        throw new NotImplementedException();
    }
}