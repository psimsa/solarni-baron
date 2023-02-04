namespace SolarniBaron.Domain.Contracts;

public enum ResponseStatus
{
    Ok,
    Error,
    Empty = 99
}

public record Result<TResponse>(ResponseStatus Status, TResponse? Data = default, string? ErrorMessage = null) ;
