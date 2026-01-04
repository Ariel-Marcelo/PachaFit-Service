using PACHA_FIT.Api.Shared;
using PACHA_FIT.Core.Domain.Dtos.Requests.User;

namespace PACHA_FIT.Core.Domain.Adapters;

public interface IAuthService
{
    public Task<Result<string>> ValidateCredentials(LoginRequest request);
    
    public Task CreateUser(NewUserRequest request);
}