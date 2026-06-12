using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
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

    public async Task<Result<UserRequests>> GetUserAsync(UserSearchCriteria criteria)
    {
        var user = await _userRepository.GetOneAsync(criteria);

        return user == null
            ? Result<UserRequests>.Failure($"Usuario no encontrado para el criterio especificado.", ErrorType.NotFound)
            : Result<UserRequests>.Success(user);
    }

    public async Task<Result<string>> UpdateUser(int userId, UserUpdateInfo updateInfo)
    {
        var user = await _userRepository.GetOneAsync(new UserSearchCriteria(userId, null, null));
        if (user == null) {
            return Result<string>.Failure("Usuario no encontrado", ErrorType.NotFound);
        }
        await _userRepository.UpdateUser(userId, updateInfo);
        return Result<string>.Success("Usuario actualizado correctamente");
    }
}