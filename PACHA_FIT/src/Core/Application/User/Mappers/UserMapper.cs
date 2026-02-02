using PACHA_FIT.Core.Domain.User.Dtos;
using Riok.Mapperly.Abstractions;
using UserEntity = PACHA_FIT.Core.Domain.Entities.User;

namespace PACHA_FIT.Core.Application.User.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class UserMapper
{
    public static partial LoginResponse UserToLoginResponse(UserEntity user, string token);

    [MapperIgnoreSource(nameof(NewUserRequest.Password))]
    public static partial UserEntity RequestToUser(NewUserRequest request, string passwordHash);

    // Método de soporte para asegurar que el hash se asigne correctamente
    // Mapperly llamará a este método automáticamente al final del mapeo de UserEntity
    private static void MapPasswordHash(string passwordHash, UserEntity target)
    {
        target.PasswordHash = passwordHash;
    }
}
