using PACHA_FIT.Core.Domain.Shared;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserService
{
    Task<Result<PachaUser>> GetUserAsync(UserSearchCriteria criteria);
    Task<Result<string>> UpdateUser(int userId, UserUpdateInfo updateInfo);
}
