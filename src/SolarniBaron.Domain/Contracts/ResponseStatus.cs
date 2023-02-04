namespace SolarniBaron.Domain.Contracts;

public enum ResponseStatus
{
    Ok,
    Error,
    Empty = 99
}

public interface IResponse
{
    ResponseStatus Status { get; }
}

public record SuccessResponse<TResponse>(TResponse Data) : IResponse
{
    public ResponseStatus Status => ResponseStatus.Ok;
}

public record ErrorResponse(string ErrorMessage) : IResponse
{
    public ResponseStatus Status => ResponseStatus.Ok;

}
