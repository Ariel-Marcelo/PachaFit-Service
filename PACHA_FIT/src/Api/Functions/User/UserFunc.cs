using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using PACHA_FIT.Api.Shared;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Api.Functions.User;

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
    [PachaAuthorize(Roles = "Admin")]
    public async Task<ResultDto<UserResponseDto>> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        string? emailFilter = req.Query["email"];
        if (string.IsNullOrEmpty(emailFilter))
        {
             return ResultDto<UserResponseDto>.Failure("El email es requerido", 400);
        }

        var filter = new UserSearchingRequest
        {
            Email = emailFilter
        };

        return await _userService.GetUserAsync(filter);
    }

    [Function("UpdateUser")]
    [PachaAuthorize]
    public async Task<ResultDto<string>> UpdateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "profile/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var currentUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId != userId)
        {
             return ResultDto<string>.Failure("No tienes permisos para modificar este perfil", 403);
        }

        var request = await req.ReadFromJsonAsync<UpdateProfileRequest>();
        if (request == null) return ResultDto<string>.Failure("Petición sin body", 400);

        ObjectValidator.Validate(request);
        
        await _userService.UpdateUser(request, userId);
        return ResultDto<string>.Success("Usuario actualizado correctamente");
    }
    
    [Function("UpdateUser")]
    [PachaAuthorize]
    public async Task<ResultDto<string>> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user/{userId}")] HttpRequest req, 
        string userId,
        FunctionContext executionContext)
    {
        var user = executionContext.Items["User"] as ClaimsPrincipal;
        var isAdmin = user?.IsInRole("Admin") ?? false;

        if (!isAdmin)
        {
            return ResultDto<string>.Failure("No tienes permisos para modificar este perfil", 403);
        }

        var request = await req.ReadFromJsonAsync<UpdateUserRequest>();
        if (request == null) return ResultDto<string>.Failure("Petición sin body", 400);

        ObjectValidator.Validate(request);
        
        await _userService.UpdateUser(request, userId);
        return ResultDto<string>.Success("Usuario actualizado correctamente");
    }
}