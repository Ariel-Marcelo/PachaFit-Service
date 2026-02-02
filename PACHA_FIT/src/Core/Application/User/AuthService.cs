using PACHA_FIT.Core.Application.User.Mappers;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
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

    public async Task<ResultDto<LoginResponse>> LoginUser(LoginRequest request)
    {
        var user = await _userRepository.GetUserRole(new UserSearchingRequest { UserName = request.Username });
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return ResultDto<LoginResponse>.Failure("Credenciales incorrectas", ErrorCodes.Unauthorized);
        }

        var token = _credentialService.GenerateToken(user);
        var response = UserMapper.UserToLoginResponse(user, token);
        return ResultDto<LoginResponse>.Success(response);
    }

    public async Task<ResultDto<string>> SignUp(NewUserRequest request)
    {
        var existingUser = await _userRepository.GetUserRole(new UserSearchingRequest { UserName = request.Email });
        if (existingUser != null)
        {
            return ResultDto<string>.Failure("El usuario ya existe", ErrorCodes.Conflict);
        }

        await _userRepository.SaveUser(UserMapper.RequestToUser(request, HashPassword(request.Password)));
        return ResultDto<string>.Success("Usuario registrado correctamente");
    }

    private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    private bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    
}