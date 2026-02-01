using PACHA_FIT.Core.Application.User.Mappers;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Application.User;

public class AuthService: IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ICredentialService _credentialService;
    private readonly UserMapper _userMapper;

    public AuthService(ICredentialService credentialService, IUserRepository userRepository, UserMapper userMapper)
    {
        _credentialService = credentialService;
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
    
    public async Task<ResultDto<LoginResponse>> LoginUser(LoginRequest request)
    {
        var user = await _userRepository.GetUserRole(new UserSearchingRequest
        {
            UserName = request.Username
        });
        
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return ResultDto<LoginResponse>.Failure("Usuario o contraseña incorrecto", ErrorCodes.Unauthorized );
        }

        var token = _credentialService.GenerateToken(user);
        
        var response = _userMapper.UserToLoginResponse(user, token);
        
        return ResultDto<LoginResponse>.Success(response);
    }

    public async Task SignUp(NewUserRequest request)
    {
        await _userRepository.SaveUser(_userMapper.RequestToUser(request, HashPassword(request.Password)));
    }
}