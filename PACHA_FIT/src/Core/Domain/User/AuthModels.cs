using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Domain.User;

public record AuthCredentials(
    string Username,
    string Password
);

public record AuthSession(
    string Email,
    string? FullName,
    string Token,
    string? RoleName
)
{
    public static Result<AuthSession> Authenticate(
        InternalUserResponse user, 
        string password, 
        IPasswordService passwordService, 
        ICredentialService credentialService)
    {
        if (passwordService.VerifyPassword(password, user.PasswordHash))
        {
            return Result<AuthSession>.Failure("Credenciales incorrectas", ErrorType.Unauthorized);
        }

        var token = credentialService.GenerateToken(user);
        var session = new AuthSession(user.Email, user.FullName, token, user.RoleName);
        return Result<AuthSession>.Success(session);
    }
}
