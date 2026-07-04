using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Api.Mappers;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PachaFitContext _context;

    public UserRepository(PachaFitContext context)
    {
        _context = context;
    }

    public async Task<UserRequests?> GetOneAsync(UserSearchCriteria criteria)
    {
        IQueryable<EntityUser> query = _context.Users.Include(u => u.Role);

        if (criteria.UserId.HasValue)
            query = query.Where(u => u.UserId == criteria.UserId.Value);

        if (!string.IsNullOrEmpty(criteria.Email))
            query = query.Where(u => u.Email == criteria.Email);

        if (!string.IsNullOrEmpty(criteria.UserName))
            query = query.Where(u => u.FullName != null && u.FullName.Contains(criteria.UserName));

        var user = await query.FirstOrDefaultAsync();
        return UserApiMapper.ToUserRequests(user);
    }

    public async Task UpdateUser(int userId, UserUpdateInfo updateInfo)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user != null)
        {
            UserApiMapper.ApplyUpdate(updateInfo, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<InternalUserResponse?> GetInternalUserAsync(string username)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == username);

        return UserApiMapper.ToInternalUser(user);
    }

    public async Task SaveUser(User user)
    {
        var entityUser = UserApiMapper.ToEntityUser(user);
        _context.Users.Add(entityUser);
        await _context.SaveChangesAsync();
    }
}