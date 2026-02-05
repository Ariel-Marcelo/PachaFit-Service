namespace PACHA_FIT.Core.Domain.User.Dtos;

public record UserResponseDto(
    int UserId,
    string Email,
    string? FullName,
    int? RoleId,
    bool? IsActive,
    DateTimeOffset? CreatedAt,
    string? IdentificationType,
    string? IdentificationNumber,
    string? Address,
    string? PhoneNumber
);