using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

namespace PACHA_FIT.Core.Domain.User.Ports;

public interface IUserService
{
    Task<ResultDto<UserResponseDto>> GetUserAsync(UserSearchingRequest filter);
    Task<ResultDto<string>> UpdateUser(UpdateProfileRequest request, string userId);
    
    Task<ResultDto<string>> UpdateUser(UpdateUserRequest request, string userId);
}