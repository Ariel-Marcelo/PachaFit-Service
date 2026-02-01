using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

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
    public async Task<ResultDto<LoginResponse>> RunLogin([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var loginData = JsonSerializer.Deserialize<LoginRequest>(body);
        
        ObjectValidator.Validate(loginData ?? throw new InvalidCastException());
        return await _authService.LoginUser(loginData);
    }

    [Function("SignUp")]
    public async Task<ResultDto<string>> RunSignUp([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var userData = JsonSerializer.Deserialize<NewUserRequest>(body);
        ObjectValidator.Validate(userData ?? throw new InvalidCastException());

        await _authService.SignUp(userData);
        return ResultDto<string>.Failure("El usuario se creó correctamente", HttpStatusCode.Created.GetHashCode());
    }
}