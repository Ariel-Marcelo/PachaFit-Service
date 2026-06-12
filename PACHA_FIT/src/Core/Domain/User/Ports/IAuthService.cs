using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IAuthService
{
    public Task<Result<AuthSession>> LoginUser(AuthCredentials credentials);
    
    public Task<Result<string>> SignUp(NewUserRegistration registration);
}
