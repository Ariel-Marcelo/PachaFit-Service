using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Application.User;

public class AuthService : IAuthService
{
    private readonly ICredentialService _credentialService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public AuthService(ICredentialService credentialService, IUserRepository userRepository,
        IPasswordService passwordService)
    {
        _credentialService = credentialService;
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<Result<AuthSession>> LoginUser(AuthCredentials credentials)
    {
        var user = await _userRepository.GetInternalUserAsync(credentials.Username);
        return user == null
            ? Result<AuthSession>.Failure("Usuario no encontrado", ErrorType.Unauthorized)
            : AuthSession.Authenticate(user!, credentials.Password, _passwordService, _credentialService);
    }

    public async Task<Result<string>> SignUp(NewUserRegistration registration)
    {
        var existingUser = await _userRepository.GetInternalUserAsync(registration.Email);
        if (existingUser != null)
        {
            return Result<string>.Failure("El usuario ya existe", ErrorType.Conflict);
        }

        var user = Domain.User.User.CreateFromRegistration(registration, _passwordService);
        await _userRepository.SaveUser(user);
        
        return Result<string>.Success("Usuario registrado correctamente");
    }
}