using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api.Shared;
using PACHA_FIT.Core.Domain.Adapters;
using PACHA_FIT.Core.Domain.Dtos.Requests.User;
using PACHA_FIT.Core.Domain.Shared;

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
    public async Task<IActionResult> RunLoginUser([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var loginData = JsonSerializer.Deserialize<LoginRequest>(body);

        ObjectValidator.Validate(loginData ?? throw new InvalidCastException());

        var result = await _authService.ValidateCredentials(loginData);

        return new OkObjectResult(result.Value);
    }

    [Function("user")]
    public async Task<IActionResult> RunCreateUser([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var userData = JsonSerializer.Deserialize<NewUserRequest>(body);

        ObjectValidator.Validate(userData ?? throw new InvalidCastException());

        await _authService.CreateUser(userData);

        return new OkObjectResult("User create successful");
    }

    [Function("user")]
    [PachaAuthorize(Roles = "Admin")]
    public  IActionResult RunGetUser([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        return new OkObjectResult("User get successful");
    }
}