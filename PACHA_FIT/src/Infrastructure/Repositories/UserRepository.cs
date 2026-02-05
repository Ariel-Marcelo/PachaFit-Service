using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Entities;
using PACHA_FIT.Core.Domain.Shared;
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

    public async Task<User?> GetOneAsync(ISpecification<User> spec)
    {
        var query = _context.Users.AsQueryable();

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(spec.Criteria);
    }

    public async Task<User> SaveUser(User user)
    {
        var savedUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return savedUser.Entity;
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}