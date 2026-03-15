using PACHA_FIT.Core.Domain.Shared;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserRepository
{
    Task<PachaUser?> GetOneAsync(UserSearchCriteria criteria);
    Task UpdateUser(int userId, UserUpdateInfo updateInfo);
    
    // Auth specific remains or also use PachaUser
    Task<Entities.User?> GetInternalUserAsync(string username);
    Task SaveUser(Entities.User user);
}
