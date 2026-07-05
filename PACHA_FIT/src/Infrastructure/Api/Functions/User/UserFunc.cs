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

namespace PACHA_FIT.Infrastructure.Api.Functions.User;

public class UserFunc
{
    private readonly ILogger<UserFunc> _logger;
    private readonly IUserService _userService;

    public UserFunc(ILogger<UserFunc> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [Function("GetUsers")]
    public async Task<Result<UserDto>> RunGetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users")] HttpRequest req)
    {
        var criteria = QueryMapper.Map<UserSearchCriteria>(req);
        return await _userService.GetUserAsync(criteria);
    }

    [Function("UpdateProfile")]
    [PachaAuthorize]
    public async Task<Result<string>> RunUpdateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/profile/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var currentUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId != userId)
        {
             return Result<string>.Failure("No tienes permisos para modificar este perfil", ErrorType.Unauthorized);
        }

        var request = await req.ReadFromJsonAsync<UpdateProfileRequest>();
        if (request == null)
        {
            return Result<string>.Failure("Invalid request body", ErrorType.BadRequest);
        }

        if (!int.TryParse(userId, out int id))
        {
            return Result<string>.Failure("Id inválido", ErrorType.BadRequest);
        }

        var updateInfo = new UserUpdateInfo(
            Email: request.Email,
            FullName: request.FullName,
            IdentificationType: request.IdentificationType,
            IdentificationNumber: request.IdentificationNumber,
            Address: request.Address,
            PhoneNumber: request.PhoneNumber
        );

        return await _userService.UpdateUser(id, updateInfo);
    }
    
    [Function("UpdateUser_Admin")]
    [PachaAuthorize]
    public async Task<Result<string>> RunUpdateUserAdmin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/user/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var isAdmin = user?.IsInRole("Admin") ?? false;

        if (!isAdmin)
        {
            return Result<string>.Failure("No tienes permisos para modificar este perfil", ErrorType.Unauthorized);
        }

        var request = await req.ReadFromJsonAsync<UpdateUserRequest>();
        if (request == null)
        {
            return Result<string>.Failure("Invalid request body", ErrorType.BadRequest);
        }

        if (!int.TryParse(userId, out int id))
        {
            return Result<string>.Failure("Id inválido", ErrorType.BadRequest);
        }

        var updateInfo = new UserUpdateInfo(
            RoleId: request.RoleId,
            IsActive: request.IsActive
        );

        return await _userService.UpdateUser(id, updateInfo);
    }
}
