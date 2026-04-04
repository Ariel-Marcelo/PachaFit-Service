using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Entities;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;

namespace PACHA_FIT.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly PachaFitContext _context;

    public UserRepository(PachaFitContext context)
    {
        _context = context;
    }

    public async Task<UserRequests?> GetOneAsync(UserSearchCriteria criteria)
    {
        IQueryable<User> query = _context.Users.Include(u => u.Role);

        if (criteria.UserId.HasValue)
            query = query.Where(u => u.UserId == criteria.UserId.Value);
        
        if (!string.IsNullOrEmpty(criteria.Email))
            query = query.Where(u => u.Email == criteria.Email);
        
        if (!string.IsNullOrEmpty(criteria.UserName))
            query = query.Where(u => u.Email == criteria.UserName);

        var user = await query.FirstOrDefaultAsync();
        return UserDomainMapper.ToDomain(user);
    }

    public async Task UpdateUser(int userId, UserUpdateInfo updateInfo)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user != null)
        {
            UserDomainMapper.ApplyUpdate(user, updateInfo);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetInternalUserAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == username);
    }

    public async Task SaveUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
