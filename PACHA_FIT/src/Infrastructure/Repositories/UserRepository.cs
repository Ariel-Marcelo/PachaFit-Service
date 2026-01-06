using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Entities;
using PACHA_FIT.Core.Domain.User.Dtos;
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

    public async Task<User?> GetUserRole(UserSearchingRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.UserName || u.FullName == request.UserName);
        return user;
    }
}