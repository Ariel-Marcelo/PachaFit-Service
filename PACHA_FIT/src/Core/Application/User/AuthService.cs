using PACHA_FIT.Core.Application.User.Mappers;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Application.User;

public class AuthService : IAuthService
{
    private readonly ICredentialService _credentialService;
    private readonly IUserRepository _userRepository;

    public AuthService(ICredentialService credentialService, IUserRepository userRepository)
    {
        _credentialService = credentialService;
        _userRepository = userRepository;
    }

    public async Task<Result<AuthSession>> LoginUser(AuthCredentials credentials)
    {
        var user = await _userRepository.GetInternalUserAsync(credentials.Username);
        if (user == null || !VerifyPassword(credentials.Password, user.PasswordHash))
        {
            return Result<AuthSession>.Failure("Credenciales incorrectas", ErrorCodes.Unauthorized);
        }

        var token = _credentialService.GenerateToken(user);
        var session = UserMapper.UserToAuthSession(user, token);
        return Result<AuthSession>.Success(session);
    }

    public async Task<Result<string>> SignUp(NewUserRegistration registration)
    {
        var existingUser = await _userRepository.GetInternalUserAsync(registration.Email);
        if (existingUser != null)
        {
            return Result<string>.Failure("El usuario ya existe", ErrorCodes.Conflict);
        }

        var user = UserMapper.RegistrationToUser(registration, HashPassword(registration.Password));
        await _userRepository.SaveUser(user);
        return Result<string>.Success("Usuario registrado correctamente");
    }

    private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    private bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    
}
