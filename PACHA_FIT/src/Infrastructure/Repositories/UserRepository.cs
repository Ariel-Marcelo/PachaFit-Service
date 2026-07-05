using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

using PACHA_FIT.Infrastructure.Repositories.UserFilters;
using UserApiMapper = PACHA_FIT.Infrastructure.Api.Mappers.UserApiMapper;

namespace PACHA_FIT.Infrastructure.Repositories;

public class UserRepository(PachaFitContext context, IEnumerable<IUserFilter> filters) : IUserRepository
{
    public async Task<UserDto?> GetOneAsync(UserSearchCriteria criteria)
    {
        IQueryable<EntityUser> query = context.Users.Include(u => u.Role);

        foreach (var filter in filters)
        {
            if (filter.CanApply(criteria))
            {
                query = filter.Apply(query, criteria);
            }
        }

        var user = await query.FirstOrDefaultAsync();
        return UserApiMapper.ToUserDto(user);
    }

    public async Task<bool> UpdateUser(int userId, UserUpdateInfo updateInfo)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return false;

        UserApiMapper.ApplyUpdate(updateInfo, user);
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<InternalUserResponse?> GetInternalUserAsync(string username)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == username);

        return UserApiMapper.ToInternalUser(user);
    }

    public async Task SaveUser(User user)
    {
        var entityUser = UserApiMapper.ToEntityUser(user);
        context.Users.Add(entityUser);
        await context.SaveChangesAsync();
    }
}