using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Application.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Result<UserDto>> GetUserAsync(UserSearchCriteria criteria)
    {
        var user = await userRepository.GetOneAsync(criteria);

        return user == null
            ? Result<UserDto>.Failure($"Usuario no encontrado para el criterio especificado.", ErrorType.NotFound)
            : Result<UserDto>.Success(user);
    }

    public async Task<Result<string>> UpdateUser(int userId, UserUpdateInfo updateInfo)
    {
        var updated = await userRepository.UpdateUser(userId, updateInfo);
        if (!updated)
        {
            return Result<string>.Failure("Usuario no encontrado", ErrorType.NotFound);
        }
        return Result<string>.Success("Usuario actualizado correctamente");
    }
}