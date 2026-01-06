using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserService
{
    public Task<Result<Entities.User>> SearchUser(UserSearchingRequest request);
}