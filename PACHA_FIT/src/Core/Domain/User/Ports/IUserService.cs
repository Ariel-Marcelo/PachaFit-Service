using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserService
{
    Task<Result<UserDto>> GetUserAsync(UserSearchCriteria criteria);

    Task<Result<string>> UpdateProfileAsync(int currentUserId, UserUpdateInfo updateInfo);

    Task<Result<string>> UpdateUserAdminAsync(int userId, UserUpdateInfo updateInfo);

    Task<Result<string>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}
