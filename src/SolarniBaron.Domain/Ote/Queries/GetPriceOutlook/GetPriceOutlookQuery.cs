using DotnetDispatcher.Core;
using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

public record GetPriceOutlookQuery(DateTime DateTime) : IQuery<Result<GetPriceOutlookQueryResponse>>;
