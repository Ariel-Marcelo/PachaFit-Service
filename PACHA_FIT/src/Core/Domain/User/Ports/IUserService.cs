using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserService
{
    Task<Result<UserDto>> GetUserAsync(UserSearchCriteria criteria);
    Task<Result<string>> UpdateUser(int userId, UserUpdateInfo updateInfo);
}
