namespace PACHA_FIT.Core.Domain.Shared;

public enum SystemError
{
    None = 0,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    Validation = 400,
    Unexpected = 500
}

public record Error(SystemError Code, string Message)
{
    public static implicit operator string?(Error? error) => error?.Message;
}
