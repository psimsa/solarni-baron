using DotnetDispatcher.Core;

namespace SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

public record GetPriceOutlookQuery() : IQuery<GetPriceOutlookQueryResponse>;

public record GetPriceOutlookQueryResponse(string Value);

public class GetPriceOutlookQueryHandler : IQueryHandler<GetPriceOutlookQuery, GetPriceOutlookQueryResponse>
{
    public Task<GetPriceOutlookQueryResponse> Query(GetPriceOutlookQuery unit, CancellationToken cancellationToken = default) =>
        Task.FromResult(new GetPriceOutlookQueryResponse("Hello world!"));
}
