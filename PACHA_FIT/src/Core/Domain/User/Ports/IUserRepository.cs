using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserRepository
{
    public Task<Entities.User?> GetUserRole(UserSearchingRequest search);
}