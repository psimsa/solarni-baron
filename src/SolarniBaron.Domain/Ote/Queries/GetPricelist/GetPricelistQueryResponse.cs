using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Contracts.Queries;

namespace SolarniBaron.Domain.Ote.Queries.GetPricelist;

public record GetPricelistQueryResponse(GetPricelistQueryResponseData? Data = null, ResponseStatus ResponseStatus = ResponseStatus.Ok, string? Error = null) :
    QueryResponse<GetPricelistQueryResponseData>(Data, ResponseStatus, Error);
