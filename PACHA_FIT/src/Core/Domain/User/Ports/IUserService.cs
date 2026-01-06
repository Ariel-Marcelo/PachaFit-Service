using PACHA_FIT.Core.Domain.Dtos.Requests.User;
using PACHA_FIT.Core.Domain.Entities;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Application;

public interface IUserService
{
    public Task<Result<User>> SearchUser(UserSearchingRequest request);
}