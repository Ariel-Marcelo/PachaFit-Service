using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserRepository
{
    Task<UserDto?> GetOneAsync(UserSearchCriteria criteria);
    
    Task<bool> UpdateUser(int userId, UserUpdateInfo updateInfo);
    
    Task<InternalUserResponse?> GetInternalUserAsync(string username);
    Task<InternalUserResponse?> GetInternalUserByIdAsync(int userId);
    
    Task<bool> UpdatePasswordAsync(int userId, string passwordHash);
    
    Task SaveUser(User user);
}
