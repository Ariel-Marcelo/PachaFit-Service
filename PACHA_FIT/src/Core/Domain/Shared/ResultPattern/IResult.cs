namespace PACHA_FIT.Core.Domain.Shared.ResultPattern;

public interface IResult
{
    bool IsSuccess { get; }
    string? Error { get; }
    ErrorType StatusCode { get; }
    object? GetValue();
}