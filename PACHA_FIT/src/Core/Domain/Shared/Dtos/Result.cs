using System.Net;

namespace PACHA_FIT.Api.Shared;

public record Result<T>(bool IsSuccess, T? Value, string? Error = null, int StatusCode = 200)
{
    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(string error, int statusCode) => new(false, default, error, statusCode);
}