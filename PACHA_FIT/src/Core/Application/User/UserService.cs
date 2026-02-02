using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

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

    public async Task<ResultDto<string>> UpdateUser(UpdateUserRequest body, string userId)
    {
        var (isSuccess, t, error, statusCode) = UpdateUserCommand.Create(userId, body);
        return !isSuccess 
            ? ResultDto<string>.Failure(error!, statusCode) 
            : ResultDto<string>.Success("Usuario actualizado correctamente");

        
        // TODO: Implement actual update logic in Repository
        // await _userRepository.UpdateUser(request);
    }
}