namespace PACHA_FIT.Core.Domain.Shared;

public interface IResult
{
    bool IsSuccess { get; }
    string? Error { get; }
    int StatusCode { get; }
    object? GetValue();
}