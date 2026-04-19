using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Infrastructure.Services;

public class BCryptPasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
