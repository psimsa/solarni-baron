namespace SolarniBaron.Domain.Contracts.Queries;

public abstract record QueryResponse<T>(T? Data, ResponseStatus ResponseStatus = ResponseStatus.Ok, string? Error = null);
