using System.Security.Claims;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface ICredentialService
{
    public ClaimsPrincipal? ValidateToken(string token);

    public string GenerateToken(Entities.User user);
}