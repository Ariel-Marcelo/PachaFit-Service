using PACHA_FIT.Core.Domain.Dtos.Requests.User;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;

namespace PACHA_FIT.Core.Application.User;

public class AuthService: IAuthService
{
    private readonly PachaFitContext _context;
    private readonly IUserRepository _userRepository;
    private readonly ICredentialService _credentialService;

    public AuthService(PachaFitContext context, ICredentialService credentialService, IUserRepository userRepository)
    {
        _context = context;
        _credentialService = credentialService;
        _userRepository = userRepository;
    }
    
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
    
    public async Task<Result<LoginResponse>> LoginUser(LoginRequest request)
    {
        var user = await _userRepository.GetUserRole(new UserSearchingRequest
        {
            UserName = request.Username
        });
        
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Failure("Usuario o contraseña incorrecto", 404 );
        }

        var token = _credentialService.GenerateToken(user);
        
        var response =  new LoginResponse
        {
            Token = token,
            FullName = user?.FullName ?? string.Empty,
            Email = user?.Email ?? string.Empty,
            RoleName = user?.Role?.Name ?? string.Empty
        };
        
        return Result<LoginResponse>.Success(response);
    }

    public async Task CreateUser(NewUserRequest request)
    {
        _context.Users.Add(new Domain.Entities.User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = HashPassword(request.Password)
        });

        await _context.SaveChangesAsync();
    }
}