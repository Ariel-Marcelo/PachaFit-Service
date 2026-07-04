using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Api.Dtos;
using UserApiMapper = PACHA_FIT.Api.Mappers.UserApiMapper;

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
    public async Task<ResultDto<LoginResponse>> RunLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/auth")] HttpRequest req)
    {
        var loginData = await req.ReadFromJsonAsync<LoginRequest>();

        if (loginData == null)
        {
             return new ResultDto<LoginResponse> { Error = "Request inválido", StatusCode = ErrorType.BadRequest };
        }
        
        CustomObjectValidator.Validate(loginData);
        
        var credentials = UserApiMapper.LoginRequestToCredentials(loginData);
        var result = await _authService.LoginUser(credentials);

        return new ResultDto<LoginResponse>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value != null ? new LoginResponse
            {
                Email = result.Value.Email,
                FullName = result.Value.FullName,
                Token = result.Value.Token,
                RoleName = result.Value.RoleName
            } : null,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    [Function("SignUp")]
    public async Task<ResultDto<string>> RunSignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/users")] HttpRequest req)
    {
        var request = await req.ReadFromJsonAsync<NewUserRequest>();
        
        if (request == null) 
        {
            return new ResultDto<string> { Error = "Petición sin body", StatusCode = ErrorType.BadRequest };
        }

        CustomObjectValidator.Validate(request);
    
        var registration = UserApiMapper.NewUserRequestToRegistration(request);
        var result = await _authService.SignUp(registration);

        return new ResultDto<string>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }
}