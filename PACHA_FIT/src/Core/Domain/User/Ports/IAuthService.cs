using PACHA_FIT.Core.Domain.Dtos.Requests.User;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IAuthService
{
    public Task<Result<LoginResponse>> LoginUser(LoginRequest request);
    
    public Task CreateUser(NewUserRequest request);
}