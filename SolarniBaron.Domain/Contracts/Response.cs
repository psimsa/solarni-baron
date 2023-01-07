namespace SolarniBaron.Domain.Contracts;

public record Response<TResponse>(TResponse Data, ResponseStatus Status, string Message);
