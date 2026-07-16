using PACHA_FIT.Core.Domain.Shared;

namespace PACHA_FIT.Core.Domain.Shared.ResultPattern;

public interface IResult
{
    bool IsSuccess { get; }
    Error? Error { get; }
    SuccessCode SuccessStatus { get; }
    object? Value { get; }
}