using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IAuthService
{
    public Task<ResultDto<LoginResponse>> LoginUser(LoginRequest request);
    
    public Task SignUp(NewUserRequest request);
}