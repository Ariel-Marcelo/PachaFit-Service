using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using PACHA_FIT.Api.Shared;
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
    public async Task<ResultDto<Core.Domain.Entities.User>> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        string? emailFilter = req.Query["email"];
        var request = new UserSearchingRequest
        {
            Email = emailFilter
        };
        return await _userService.SearchUser(request);
    }

}