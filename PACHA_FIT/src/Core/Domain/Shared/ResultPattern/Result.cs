namespace PACHA_FIT.Core.Domain.Shared.ResultPattern;

public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public ErrorType StatusCode { get; }
    public T? Value { get; }

    protected Result(T? value, string? error, ErrorType statusCode)
    {
        IsSuccess = false;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }
    
    protected Result(T? value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    public object? GetValue() => Value;

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error, ErrorType statusCode = ErrorType.BadRequest) => new(default, error, statusCode);
    
    // Allow implicit conversion to ResultDto for backward compatibility or API layer mapping if needed
    // But since middleware uses IResult, we are good.
}
