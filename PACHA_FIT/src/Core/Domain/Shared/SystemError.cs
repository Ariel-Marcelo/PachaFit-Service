namespace PACHA_FIT.Core.Domain.Shared;

public enum SystemError
{
    None = 0,
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    Validation = 400,
    Unexpected = 500,

    // Security & Identity Error Codes
    UserNotFound = 2000,
    UserAlreadyExists = 2001,
    InvalidCredentials = 2002,
    Unauthorized = 2003
}

public record Error(SystemError Code, string Message)
{
    public static implicit operator string?(Error? error) => error?.Message;
}

public enum SuccessCode
{
    Ok = 200,
    Created = 201,
    NoContent = 204
}

