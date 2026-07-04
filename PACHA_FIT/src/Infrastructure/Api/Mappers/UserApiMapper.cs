using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Infrastructure.Api.Dtos;
using Riok.Mapperly.Abstractions;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class UserApiMapper
{
    // To Domain
    [MapProperty(nameof(EntityUser.Role) + "." + nameof(EntityUser.Role.Name), "RoleName")]
    public static partial InternalUserResponse? ToInternalUser(EntityUser? user);
    
    public static partial AuthCredentials LoginRequestToCredentials(LoginRequest request);

    public static partial NewUserRegistration NewUserRequestToRegistration(NewUserRequest request);
    
    [MapProperty(nameof(EntityUser.Role) + "." + nameof(EntityUser.Role.Name), "RoleName")]
    public static partial UserRequests? ToUserRequests(EntityUser? user);
    
    
    // To Response
    public static partial UserResponseDto ToUserResponseDto(UserRequests user);

    
    // ToEntities
    public static partial EntityUser ToEntityUser(User user);
    
    
    //Utils
    public static partial void ApplyUpdate(UserUpdateInfo updateInfo, EntityUser user);
    
    
    }