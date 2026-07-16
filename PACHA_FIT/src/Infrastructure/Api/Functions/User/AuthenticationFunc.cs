using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Infrastructure.Api.Dtos;
using UserApiMapper = PACHA_FIT.Infrastructure.Api.Mappers.UserApiMapper;

namespace PACHA_FIT.Infrastructure.Api.Functions.User;

public class AuthenticationFunc(ILogger<AuthenticationFunc> logger, IAuthService authService)
{
    private readonly ILogger<AuthenticationFunc> _logger = logger;

    [Function("authentication")]
    public async Task<Result<AuthSession>> RunLogin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/auth")] HttpRequest req)
    {
        var credentials = await req.ReadFromJsonAsync<AuthCredentials>();

        if (credentials == null)
            return Result<AuthSession>.Failure(new Error(SystemError.BadRequest, CommonMessages.InvalidRequestBody));

        if (string.IsNullOrEmpty(credentials.Username) || string.IsNullOrEmpty(credentials.Password))
        {
            return Result<AuthSession>.Failure(new Error(SystemError.BadRequest, "El usuario y la contraseña son requeridos"));
        }

        var result = await authService.LoginUser(credentials);

        if (!result.IsSuccess)
            return Result<AuthSession>.Failure(result.Error ?? new Error(SystemError.Unexpected, "Error al iniciar sesión"));

        return result;
    }

    [Function("SignUp")]
    public async Task<Result<string>> RunSignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/users")] HttpRequest req)
    {
        var request = await req.ReadFromJsonAsync<NewUserRequest>();

        if (request == null)
        {
            return Result<string>.Failure(new Error(SystemError.BadRequest, CommonMessages.InvalidRequestBody));
        }

        CustomObjectValidator.Validate(request);

        var registration = UserApiMapper.NewUserRequestToRegistration(request);
        return await authService.SignUp(registration);
    }
}