using System.Net;
using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;

namespace PACHA_FIT.Core.Application.User;

public class UserService : IUserService
{
    private readonly PachaFitContext _context;

    public UserService(PachaFitContext context)
    {
        _context = context;
    }

    public async Task<Result<Domain.Entities.User>> SearchUser(UserSearchingRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        return user == null
            ? Result<Domain.Entities.User>.Failure("Usuario no encontrado", HttpStatusCode.NotFound.GetHashCode())
            : Result<Domain.Entities.User>.Success(user);
    }
}