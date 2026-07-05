using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Infrastructure.Api.Dtos;
using Riok.Mapperly.Abstractions;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Infrastructure.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class UserApiMapper
{
    // To Domain
    [MapProperty(nameof(EntityUser.Role) + "." + nameof(EntityUser.Role.Name), "RoleName")]
    public static partial InternalUserResponse? ToInternalUser(EntityUser? user);
    


    public static partial NewUserRegistration NewUserRequestToRegistration(NewUserRequest request);
    
    public static partial UserUpdateInfo ToUpdateInfo(UpdateProfileRequest request);
    public static partial UserUpdateInfo ToUpdateInfo(UpdateUserRequest request);
    
    [MapProperty(nameof(EntityUser.Role) + "." + nameof(EntityUser.Role.Name), "RoleName")]
    public static partial UserDto? ToUserDto(EntityUser? user);

    
    // ToEntities
    public static partial EntityUser ToEntityUser(User user);
    
    
    //Utils
    public static void ApplyUpdate(UserUpdateInfo updateInfo, EntityUser user)
    {
        if (updateInfo.Email != null) user.Email = updateInfo.Email;
        if (updateInfo.FullName != null) user.FullName = updateInfo.FullName;
        if (updateInfo.IdentificationType != null) user.IdentificationType = updateInfo.IdentificationType;
        if (updateInfo.IdentificationNumber != null) user.IdentificationNumber = updateInfo.IdentificationNumber;
        if (updateInfo.Address != null) user.Address = updateInfo.Address;
        if (updateInfo.PhoneNumber != null) user.PhoneNumber = updateInfo.PhoneNumber;
        if (updateInfo.RoleId.HasValue) user.RoleId = updateInfo.RoleId.Value;
        if (updateInfo.IsActive.HasValue) user.IsActive = updateInfo.IsActive.Value;
    }
    
    
    }