using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Persistence;

namespace PACHA_FIT.Core.Application.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResultDto<Domain.Entities.User>> SearchUser(UserSearchingRequest request)
    {
        var user = await _userRepository.GetUserRole(request);
        return user == null
            ? ResultDto<Domain.Entities.User>.Failure("Usuario no encontrado", ErrorCodes.NotFound)
            : ResultDto<Domain.Entities.User>.Success(user);
    }
}