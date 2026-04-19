namespace PACHA_FIT.Core.Domain.User.Dtos;

public record UserResponses();

public record InternalUserResponse(
    int UserId,
    string Email,
    string? FullName,
    string PasswordHash,
    string? RoleName
);
