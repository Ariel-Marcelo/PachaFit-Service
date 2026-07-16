using PACHA_FIT.Core.Domain.Shared;

namespace PACHA_FIT.Core.Domain.Shared.ResultPattern;

public abstract record Result<T> : IResult
{
    public bool IsSuccess { get; protected init; }
    public Error? Error { get; protected init; }
    public T? Value { get; protected init; }
    public SuccessCode SuccessStatus { get; protected init; } = SuccessCode.Ok;

    object? IResult.Value => Value;

    public static Result<T> Success(T value) => new Success<T>(value);

    public static Result<T> Failure(Error error) => new Failure<T>(error);

    public static Result<T> Created(T value) => new Success<T>(value) { SuccessStatus = SuccessCode.Created };

    public static Result<T> NoContent() => new Success<T>(default!) { SuccessStatus = SuccessCode.NoContent };
}

public record Success<T> : Result<T>
{
    public Success(T value)
    {
        Value = value;
        IsSuccess = true;
    }
}

public record Failure<T> : Result<T>
{
    public Failure(Error error)
    {
        IsSuccess = false;
        Error = error;
    }
}

