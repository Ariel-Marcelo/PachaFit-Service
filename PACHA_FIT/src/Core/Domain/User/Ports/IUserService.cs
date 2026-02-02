using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserService
{
    public Task<ResultDto<Entities.User>> SearchUser(UserSearchingRequest request);
    Task<ResultDto<string>> UpdateUser(UpdateUserRequest request, string userId);
}