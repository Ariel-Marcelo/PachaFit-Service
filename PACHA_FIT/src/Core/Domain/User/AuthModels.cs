namespace PACHA_FIT.Core.Domain.User;

public record AuthCredentials(
    string Username,
    string Password
);

public record NewUserRegistration(
    string Email,
    string Password,
    string FullName
);

public record AuthSession(
    string Email,
    string FullName,
    string Token,
    string? RoleName
);
