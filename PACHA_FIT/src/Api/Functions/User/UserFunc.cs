using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PACHA_FIT.Api.Shared;
using PACHA_FIT.Core.Application;
using PACHA_FIT.Core.Domain.Shared.Dtos;
using PACHA_FIT.Core.Domain.User.Dtos;

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
    public async Task<Result<Core.Domain.Entities.User>> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        string? emailFilter = req.Query["email"];
        var request = new UserSearchingRequest
        {
            Email = emailFilter
        };
        var result = await _userService.SearchUser(request);
        return result;
    }

}