namespace PACHA_FIT.Core.Domain.User.Dtos;

public record UserDto(
    int UserId,
    string Email,
    string? FullName,
    int? RoleId,
    bool? IsActive,
    DateTimeOffset? CreatedAt,
    string? IdentificationType,
    string? IdentificationNumber,
    string? Address,
    string? PhoneNumber,
    string? RoleName = null
);

public record UserSearchCriteria
{
    public int? UserId { get; init; }
    public string? Email { get; init; }
    public string? UserName { get; init; }
}

public record UserUpdateInfo(
    string? Email = null,
    string? FullName = null,
    string? IdentificationType = null,
    string? IdentificationNumber = null,
    string? Address = null,
    string? PhoneNumber = null,
    int? RoleId = null,
    bool? IsActive = null
);

public record NewUserRegistration(
    string Email,
    string Password,
    string FullName
);
