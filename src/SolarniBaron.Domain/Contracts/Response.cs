namespace SolarniBaron.Domain.Contracts;

public record ApiResponse<TResponse>(TResponse Data, ResponseStatus Status, string? Message = null);
