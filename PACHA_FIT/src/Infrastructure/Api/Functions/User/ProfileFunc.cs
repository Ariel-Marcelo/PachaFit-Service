using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Api.Dtos;
using UserApiMapper = PACHA_FIT.Infrastructure.Api.Mappers.UserApiMapper;

namespace PACHA_FIT.Infrastructure.Api.Functions.User;

public class ProfileFunc(IUserService userService)
{
    [Function("UpdateProfile")]
    [PachaAuthorize]
    public async Task<Result<string>> RunUpdateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/profile")] HttpRequest req, 
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var currentUserIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(currentUserIdStr, out int currentUserId))
        {
             return Result<string>.Failure(CommonMessages.UnauthorizedAccess, ErrorType.Unauthorized);
        }

        var request = await req.ReadFromJsonAsync<UpdateProfileRequest>();
        if (request == null)
        {
            return Result<string>.Failure(CommonMessages.InvalidRequestBody, ErrorType.BadRequest);
        }
        CustomObjectValidator.Validate(request);

        var updateInfo = UserApiMapper.ToUpdateInfo(request);

        return await userService.UpdateProfileAsync(currentUserId, updateInfo);
    }

    [Function("ChangePassword")]
    [PachaAuthorize]
    public async Task<Result<string>> RunChangePassword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/profile/change-password")] HttpRequest req, 
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var currentUserIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(currentUserIdStr, out int currentUserId))
        {
             return Result<string>.Failure(CommonMessages.UnauthorizedAccess, ErrorType.Unauthorized);
        }

        var request = await req.ReadFromJsonAsync<ChangePasswordRequest>();
        if (request == null)
        {
            return Result<string>.Failure(CommonMessages.InvalidRequestBody, ErrorType.BadRequest);
        }
        CustomObjectValidator.Validate(request);

        return await userService.ChangePasswordAsync(currentUserId, request.CurrentPassword, request.NewPassword);
    }
}
