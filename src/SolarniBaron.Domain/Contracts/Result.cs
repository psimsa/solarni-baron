namespace SolarniBaron.Domain.Contracts;

public record Result<TResponse>(ResponseStatus Status, TResponse? Data = default, string? ErrorMessage = null)
{
    public Result(TResponse data) : this(ResponseStatus.Ok, data) { }
    public Result(string errorMessage) : this(ResponseStatus.Error, default, errorMessage) { }
}
