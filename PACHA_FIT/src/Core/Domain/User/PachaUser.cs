namespace PACHA_FIT.Core.Domain.User;

public record PachaUser(
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

public record UserSearchCriteria(
    int? UserId,
    string? Email,
    string? UserName
);

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
