namespace PACHA_FIT.Core.Domain.Shared;

public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public int StatusCode { get; }
    public T? Value { get; }

    protected Result(bool isSuccess, T? value, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public object? GetValue() => Value;

    public static Result<T> Success(T value) => new(true, value, null, 200);
    public static Result<T> Failure(string error, int statusCode = 400) => new(false, default, error, statusCode);
    
    // Allow implicit conversion to ResultDto for backward compatibility or API layer mapping if needed
    // But since middleware uses IResult, we are good.
}

public class Result : IResult
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public int StatusCode { get; }

    protected Result(bool isSuccess, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public object? GetValue() => null;

    public static Result Success() => new(true, null, 200);
    public static Result Failure(string error, int statusCode = 400) => new(false, error, statusCode);
}
