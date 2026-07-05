using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api;
using PACHA_FIT.Api.Mappers;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Api.Dtos;
using UserApiMapper = PACHA_FIT.Infrastructure.Api.Mappers.UserApiMapper;

namespace PACHA_FIT.Infrastructure.Api.Functions.User;

public class AdminUserFunc(IUserService userService)
{
    [Function("GetUsers")]
    [PachaAuthorize(Roles = "Admin")] // Only admins should search users
    public async Task<Result<UserDto>> RunGetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users")] HttpRequest req)
    {
        var criteria = QueryMapper.Map<UserSearchCriteria>(req);
        return await userService.GetUserAsync(criteria);
    }

    [Function("UpdateUser_Admin")]
    [PachaAuthorize(Roles = "Admin")]
    public async Task<Result<string>> RunUpdateUserAdmin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/user/{userId:int}")] HttpRequest req, 
        int userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var isAdmin = user?.IsInRole("Admin") ?? false;

        if (!isAdmin)
        {
            return Result<string>.Failure(CommonMessages.UnauthorizedAccess, ErrorType.Unauthorized);
        }

        var request = await req.ReadFromJsonAsync<UpdateUserRequest>();
        if (request == null)
        {
            return Result<string>.Failure(CommonMessages.InvalidRequestBody, ErrorType.BadRequest);
        }

        CustomObjectValidator.Validate(request);

        var updateInfo = UserApiMapper.ToUpdateInfo(request);

        return await userService.UpdateUserAdminAsync(userId, updateInfo);
    }
}
