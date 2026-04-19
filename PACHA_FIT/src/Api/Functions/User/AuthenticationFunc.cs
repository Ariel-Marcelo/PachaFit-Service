using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Nswag;
using UserApiMapper = PACHA_FIT.Api.Mappers.UserApiMapper;

namespace PACHA_FIT.Api.Functions.User;

public class AuthenticationFunc : AuthControllerBase
{
    private readonly ILogger<AuthenticationFunc> _logger;
    private readonly IAuthService _authService;

    public AuthenticationFunc(ILogger<AuthenticationFunc> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [Function("authentication")]
    public async Task<ResultDtoOfLoginResponse> RunLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/auth")] HttpRequest req)
    {
        var loginData = await req.ReadFromJsonAsync<LoginRequest>();

        if (loginData == null)
        {
             return new ResultDtoOfLoginResponse { Error = "Request inválido", StatusCode = ErrorType.BadRequest };
        }
        
        CustomObjectValidator.Validate(loginData);
        
        return await this.Login(loginData);
    }

    [Function("SignUp")]
    public async Task<ResultDtoOfString> RunSignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/users")] HttpRequest req)
    {
        var request = await req.ReadFromJsonAsync<NewUserRequest>();
        
        if (request == null) 
        {
            return new ResultDtoOfString { Error = "Petición sin body", StatusCode = ErrorType.BadRequest };
        }

        CustomObjectValidator.Validate(request);
    
        return await this.SignUp(request);
    }

    public override async Task<ResultDtoOfLoginResponse> Login(LoginRequest body, CancellationToken cancellationToken = default)
    {
        var credentials = UserApiMapper.LoginRequestToCredentials(body);
        var result = await _authService.LoginUser(credentials);

        return new ResultDtoOfLoginResponse
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

    public override async Task<ResultDtoOfString> SignUp(NewUserRequest body, CancellationToken cancellationToken = default)
    {
        var registration = UserApiMapper.NewUserRequestToRegistration(body);
        var result = await _authService.SignUp(registration);

        return new ResultDtoOfString
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }
}
    