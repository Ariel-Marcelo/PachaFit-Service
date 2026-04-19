namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
