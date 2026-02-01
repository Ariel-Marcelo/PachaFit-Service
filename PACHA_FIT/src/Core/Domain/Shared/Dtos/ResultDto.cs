namespace PACHA_FIT.Core.Domain.Shared.Dtos;

public record ResultDto<T>(bool IsSuccess, T? Value, string? Error = null, int StatusCode = 200) : IResult
{
    public object? GetValue() => Value;
    public static ResultDto<T> Success(T value) => new(true, value);
    public static ResultDto<T> Failure(string error, int statusCode) => new(false, default, error, statusCode);
    
}