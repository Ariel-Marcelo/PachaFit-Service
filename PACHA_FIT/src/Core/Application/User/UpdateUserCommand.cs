using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Application.User;

public record UpdateUserCommand(
    Guid Id,
    string Email,
    string Password,
    string FullName,
    string IdentificationType,
    string IdentificationNumber,
    string Address,
    string PhoneNumber,
    string RoleId
)
{
    public static ResultDto<UpdateUserCommand> Create(string userId, UpdateUserRequest request)
    {
        if (!Guid.TryParse(userId, out var userGuid))
            return ResultDto<UpdateUserCommand>.Failure("El ID de usuario no tiene un formato válido", 400);

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
            return ResultDto<UpdateUserCommand>.Failure("Email inválido", 400);

        if (request.Password?.Length < 8)
            return ResultDto<UpdateUserCommand>.Failure("La contraseña debe tener al menos 8 caracteres", 400);

        return ResultDto<UpdateUserCommand>.Success(new UpdateUserCommand(
            userGuid,
            request.Email,
            request.Password,
            request.FullName,
            request.IdentificationType,
            request.IdentificationNumber,
            request.Address,
            request.PhoneNumber,
            request.RoleId
        ));
    }
}