using PACHA_FIT.Core.Domain.User;
using Riok.Mapperly.Abstractions;
using UserEntity = PACHA_FIT.Core.Domain.Entities.User;

namespace PACHA_FIT.Core.Application.User.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class UserMapper
{
    public static partial AuthSession UserToAuthSession(UserEntity user, string token);

    [MapperIgnoreSource(nameof(NewUserRegistration.Password))]
    public static partial UserEntity RegistrationToUser(NewUserRegistration registration, string passwordHash);
    
    public static partial void UpdateUserFromDomain(UserUpdateInfo info, UserEntity target);

    public static partial UserRequests UserToDomain(UserEntity user);

    // Método de soporte para asegurar que el hash se asigne correctamente
    private static void MapPasswordHash(string passwordHash, UserEntity target)
    {
        target.PasswordHash = passwordHash;
    }
}
