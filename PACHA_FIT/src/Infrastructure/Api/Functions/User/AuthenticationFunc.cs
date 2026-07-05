using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Api.Dtos;
using UserApiMapper = PACHA_FIT.Infrastructure.Api.Mappers.UserApiMapper;

namespace PACHA_FIT.Api.Functions.User;

public class AuthenticationFunc
{
    private readonly ILogger<AuthenticationFunc> _logger;
    private readonly IAuthService _authService;

    public AuthenticationFunc(ILogger<AuthenticationFunc> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [Function("authentication")]
    public async Task<Result<LoginResponse>> RunLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/auth")] HttpRequest req)
    {
        var loginData = await req.ReadFromJsonAsync<LoginRequest>();

        if (loginData == null)
        {
             return Result<LoginResponse>.Failure("Request inválido", ErrorType.BadRequest);
        }
        
        CustomObjectValidator.Validate(loginData);
        
        var credentials = UserApiMapper.LoginRequestToCredentials(loginData);
        var result = await _authService.LoginUser(credentials);

        if (!result.IsSuccess || result.Value == null)
        {
            return Result<LoginResponse>.Failure(result.Error ?? "Error al iniciar sesión", result.StatusCode);
        }

        var response = new LoginResponse
        {
            Email = result.Value.Email,
            FullName = result.Value.FullName,
            Token = result.Value.Token,
            RoleName = result.Value.RoleName
        };

        return Result<LoginResponse>.Success(response);
    }

    [Function("SignUp")]
    public async Task<Result<string>> RunSignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/users")] HttpRequest req)
    {
        var request = await req.ReadFromJsonAsync<NewUserRequest>();
        
        if (request == null) 
        {
            return Result<string>.Failure("Petición sin body", ErrorType.BadRequest);
        }

        CustomObjectValidator.Validate(request);
    
        var registration = UserApiMapper.NewUserRequestToRegistration(request);
        return await _authService.SignUp(registration);
    }
}