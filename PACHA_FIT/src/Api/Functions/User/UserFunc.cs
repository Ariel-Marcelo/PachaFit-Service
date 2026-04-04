using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using PACHA_FIT.Api.Shared;
using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.Generated;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Api.Functions.User;

public class UserFunc : UserControllerBase
{
    private readonly ILogger<UserFunc> _logger;
    private readonly IUserService _userService;

    public UserFunc(ILogger<UserFunc> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [Function("GetUsers")]
    public async Task<ResultDtoOfUserResponseDto> RunGetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users")] HttpRequest req)
    {
        SearchingUser filters = QueryMapper.Map<SearchingUser>(req);
        return await this.GetUsers(filters.Email);
    }

    [Function("UpdateProfile")]
    [PachaAuthorize]
    public async Task<ResultDtoOfString> RunUpdateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/profile/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var currentUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId != userId)
        {
             return new ResultDtoOfString { IsSuccess = false, Error = "No tienes permisos para modificar este perfil", StatusCode = 403 };
        }

        var request = await req.ReadFromJsonAsync<UpdateProfileRequest>();
        return await this.UpdateProfile(userId, request);
    }
    
    [Function("UpdateUser_Admin")]
    [PachaAuthorize]
    public async Task<ResultDtoOfString> RunUpdateUserAdmin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/user/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var isAdmin = user?.IsInRole("Admin") ?? false;

        if (!isAdmin)
        {
            return new ResultDtoOfString { IsSuccess = false, Error = "No tienes permisos para modificar este perfil", StatusCode = 403 };
        }

        var request = await req.ReadFromJsonAsync<UpdateUserRequest>();
        return await this.UpdateUser(userId, request);
    }

    public override async Task<ResultDtoOfUserResponseDto> GetUsers(string email, CancellationToken cancellationToken = default)
    {
        var criteria = new UserSearchCriteria(null, email, null);
        var result = await _userService.GetUserAsync(criteria);

        return new ResultDtoOfUserResponseDto
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value != null ? MapToResponse(result.Value) : null,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    public override async Task<ResultDtoOfString> UpdateProfile(string userId, UpdateProfileRequest body, CancellationToken cancellationToken = default)
    {
        if (!int.TryParse(userId, out int id)) return new ResultDtoOfString { IsSuccess = false, Error = "Id inválido", StatusCode = 400 };

        var updateInfo = new UserUpdateInfo(
            Email: body.Email,
            FullName: body.FullName,
            IdentificationType: body.IdentificationType,
            IdentificationNumber: body.IdentificationNumber,
            Address: body.Address,
            PhoneNumber: body.PhoneNumber
        );

        var result = await _userService.UpdateUser(id, updateInfo);
        
        return new ResultDtoOfString
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    public override async Task<ResultDtoOfString> UpdateUser(string userId, UpdateUserRequest body, CancellationToken cancellationToken = default)
    {
        if (!int.TryParse(userId, out int id)) return new ResultDtoOfString { IsSuccess = false, Error = "Id inválido", StatusCode = 400 };

        var updateInfo = new UserUpdateInfo(
            RoleId: body.RoleId,
            IsActive: body.IsActive
        );

        var result = await _userService.UpdateUser(id, updateInfo);

        return new ResultDtoOfString
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    private PACHA_FIT.Core.Domain.Generated.UserResponseDto MapToResponse(UserRequests userRequests)
    {
        return new PACHA_FIT.Core.Domain.Generated.UserResponseDto
        {
            UserId = userRequests.UserId,
            Email = userRequests.Email,
            FullName = userRequests.FullName,
            RoleId = userRequests.RoleId,
            IsActive = userRequests.IsActive,
            CreatedAt = userRequests.CreatedAt,
            IdentificationType = userRequests.IdentificationType,
            IdentificationNumber = userRequests.IdentificationNumber,
            Address = userRequests.Address,
            PhoneNumber = userRequests.PhoneNumber
        };
    }
}
