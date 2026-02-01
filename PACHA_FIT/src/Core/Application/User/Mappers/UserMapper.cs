using PACHA_FIT.Core.Domain.User.Dtos;
using Riok.Mapperly.Abstractions;
using UserEntity = PACHA_FIT.Core.Domain.Entities.User;

namespace PACHA_FIT.Core.Application.User.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class UserMapper
{
    public partial LoginResponse UserToLoginResponse(UserEntity user, string token);

    [MapperIgnoreSource(nameof(NewUserRequest.Password))]
    public partial UserEntity RequestToUser(NewUserRequest request, string passwordHash);

    // Método de soporte para asegurar que el hash se asigne correctamente
    // Mapperly llamará a este método automáticamente al final del mapeo de UserEntity
    private void MapPasswordHash(string passwordHash, UserEntity target)
    {
        target.PasswordHash = passwordHash;
    }
}
