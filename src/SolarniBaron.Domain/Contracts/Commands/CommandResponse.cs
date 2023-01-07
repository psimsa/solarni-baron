namespace SolarniBaron.Domain.Contracts.Commands;

public record CommandResponse<T>(T? Data, ResponseStatus ResponseStatus = ResponseStatus.Ok, string? Error = null)
{
    public static CommandResponse<T> Empty() => new(default, ResponseStatus.Empty, null);
}
