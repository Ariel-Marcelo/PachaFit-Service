namespace PACHA_FIT.Core.Domain.Shared;

public enum ErrorType
{
    Success = 200,
    NotFound = 404,
    Unauthorized = 401,
    Validation = 400,
    Conflict = 409,
    Unexpected = 500,
    BadRequest = 400
}