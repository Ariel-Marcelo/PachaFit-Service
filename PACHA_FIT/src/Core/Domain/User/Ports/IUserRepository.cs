using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserRepository
{
    Task<Entities.User?> GetOneAsync(ISpecification<Entities.User> spec);
    Task<Entities.User> SaveUser(Entities.User user);
    Task UpdateUser(Entities.User user);
}