using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserRepository
{
    Task<UserRequests?> GetOneAsync(UserSearchCriteria criteria);
    Task UpdateUser(int userId, UserUpdateInfo updateInfo);
    
    Task<InternalUserResponse?> GetInternalUserAsync(string username);
    
    Task SaveUser(User user);
}
