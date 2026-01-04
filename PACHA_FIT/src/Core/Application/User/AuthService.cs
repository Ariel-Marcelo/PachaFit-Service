using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Api.Shared;
using PACHA_FIT.Core.Domain.Adapters;
using PACHA_FIT.Core.Domain.Dtos.Requests.User;
using PACHA_FIT.Core.Domain.Entities;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Core.Application;

public class AuthService: IAuthService
{
    private readonly PachaFitContext _context;
    private readonly ICredentialService _credentialService;

    public AuthService(PachaFitContext context, ICredentialService credentialService)
    {
        _context = context;
        _credentialService = credentialService;
    }
    
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
    
    public async Task<Result<string>> ValidateCredentials(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Username || u.FullName == request.Username);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }
        var token = _credentialService.GenerateToken(user);
        return Result<string>.Success(token);
    }

    public async Task CreateUser(NewUserRequest request)
    {
        _context.Users.Add(new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = HashPassword(request.Password)
        });

        await _context.SaveChangesAsync();
    }
}