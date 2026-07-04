using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api;
using PACHA_FIT.Api.Mappers;
using PACHA_FIT.Core.Domain.Shared;
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
    public async Task<ResultDto<UserResponseDto>> RunGetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users")] HttpRequest req)
    {
        var criteria = QueryMapper.Map<UserSearchCriteria>(req);
        var result = await _userService.GetUserAsync(criteria);

        return new ResultDto<UserResponseDto>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value != null ? UserApiMapper.ToUserResponseDto(result.Value) : null,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    [Function("UpdateProfile")]
    [PachaAuthorize]
    public async Task<ResultDto<string>> RunUpdateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/profile/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var currentUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId != userId)
        {
             return new ResultDto<string> { IsSuccess = false, Error = "No tienes permisos para modificar este perfil", StatusCode = ErrorType.Unauthorized };
        }

        var request = await req.ReadFromJsonAsync<UpdateProfileRequest>();
        if (request == null)
        {
            return new ResultDto<string> { IsSuccess = false, Error = "Invalid request body", StatusCode = ErrorType.BadRequest };
        }

        if (!int.TryParse(userId, out int id))
        {
            return new ResultDto<string> { IsSuccess = false, Error = "Id inválido", StatusCode = ErrorType.BadRequest };
        }

        var updateInfo = new UserUpdateInfo(
            Email: request.Email,
            FullName: request.FullName,
            IdentificationType: request.IdentificationType,
            IdentificationNumber: request.IdentificationNumber,
            Address: request.Address,
            PhoneNumber: request.PhoneNumber
        );

        var result = await _userService.UpdateUser(id, updateInfo);
        
        return new ResultDto<string>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }
    
    [Function("UpdateUser_Admin")]
    [PachaAuthorize]
    public async Task<ResultDto<string>> RunUpdateUserAdmin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/user/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var isAdmin = user?.IsInRole("Admin") ?? false;

        if (!isAdmin)
        {
            return new ResultDto<string> { IsSuccess = false, Error = "No tienes permisos para modificar este perfil", StatusCode = ErrorType.Unauthorized };
        }

        var request = await req.ReadFromJsonAsync<UpdateUserRequest>();
        if (request == null)
        {
            return new ResultDto<string> { IsSuccess = false, Error = "Invalid request body", StatusCode = ErrorType.BadRequest };
        }

        if (!int.TryParse(userId, out int id))
        {
            return new ResultDto<string> { IsSuccess = false, Error = "Id inválido", StatusCode = ErrorType.BadRequest };
        }

        var updateInfo = new UserUpdateInfo(
            RoleId: request.RoleId,
            IsActive: request.IsActive
        );

        var result = await _userService.UpdateUser(id, updateInfo);

        return new ResultDto<string>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }
}
