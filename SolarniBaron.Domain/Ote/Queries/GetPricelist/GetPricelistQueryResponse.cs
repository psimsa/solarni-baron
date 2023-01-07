using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponse()
{
    public static GetPricelistQueryResponse Empty() => new GetPricelistQueryResponse();
};

public class GetPricelistQueryHandler : IQueryHandler<IQuery<GetPricelistQueryResponse>, GetPricelistQueryResponse>
{
    public Task<GetPricelistQueryResponse> Get(IQuery<GetPricelistQueryResponse> query)
    {
        throw new NotImplementedException();
    }
}