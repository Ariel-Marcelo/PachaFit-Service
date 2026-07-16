using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Application.User;

public class UserService(IUserRepository userRepository, IPasswordService passwordService) : IUserService
{
    public async Task<Result<UserDto>> GetUserAsync(UserSearchCriteria criteria)
    {
        var user = await userRepository.GetOneAsync(criteria);

        return user == null
            ? Result<UserDto>.Failure(new Error(SystemError.UserNotFound, "Usuario no encontrado para el criterio especificado."))
            : Result<UserDto>.Success(user);
    }

    public async Task<Result<string>> UpdateProfileAsync(int currentUserId, UserUpdateInfo updateInfo)
    {
        var updated = await userRepository.UpdateUser(currentUserId, updateInfo);
        if (!updated)
        {
            return Result<string>.Failure(new Error(SystemError.UserNotFound, CommonMessages.UserNotFound));
        }
        return Result<string>.Success("Usuario actualizado correctamente");
    }

    public async Task<Result<string>> UpdateUserAdminAsync(int userId, UserUpdateInfo updateInfo)
    {
        var updated = await userRepository.UpdateUser(userId, updateInfo);
        if (!updated)
        {
            return Result<string>.Failure(new Error(SystemError.UserNotFound, CommonMessages.UserNotFound));
        }
        return Result<string>.Success("Usuario actualizado correctamente");
    }

    public async Task<Result<string>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await userRepository.GetInternalUserByIdAsync(userId);
        if (user == null)
        {
            return Result<string>.Failure(new Error(SystemError.UserNotFound, CommonMessages.UserNotFound));
        }

        if (!passwordService.VerifyPassword(currentPassword, user.PasswordHash))
        {
            return Result<string>.Failure(new Error(SystemError.InvalidCredentials, CommonMessages.Auth.InvalidCredentials));
        }

        if (!IsPasswordStrong(newPassword))
        {
            return Result<string>.Failure(new Error(SystemError.Validation, CommonMessages.Validation.PasswordTooWeak));
        }

        var newHash = passwordService.HashPassword(newPassword);
        var updated = await userRepository.UpdatePasswordAsync(userId, newHash);

        return updated
            ? Result<string>.Success(CommonMessages.Auth.PasswordChangedSuccess)
            : Result<string>.Failure(new Error(SystemError.UserNotFound, CommonMessages.UserNotFound));
    }

    private static bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        bool hasUpper = false;
        bool hasDigit = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpper = true;
            if (char.IsDigit(c)) hasDigit = true;

            if (hasUpper && hasDigit) return true;
        }

        return false;
    }
}