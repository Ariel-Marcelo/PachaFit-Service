using System.Security.Claims;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface ICredentialService
{
    public ClaimsPrincipal? ValidateToken(string token);

    public string GenerateToken(InternalUserResponse user);
}