using Reqnroll;
using NSubstitute;
using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.BddTests.Steps.User;

[Binding]
public class UserServiceSteps
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly UserService _userService;
    private Result<UserRequests>? _getUserResult;
    private Result<string>? _updateUserResult;

    public UserServiceSteps()
    {
        _userService = new UserService(_userRepository);
    }

    [Given(@"a user exists with ID (.*) and email ""(.*)""")]
    public void GivenAUserExistsWithIDAndEmail(int userId, string email)
    {
        var user = new UserRequests(userId, email, "Test User", 1, true, DateTimeOffset.Now, "DNI", "12345678", "Address", "123456789");
        _userRepository.GetOneAsync(Arg.Is<UserSearchCriteria>(c => c.UserId == userId)).Returns(Task.FromResult<UserRequests?>(user));
    }

    [Given(@"a user exists with ID (.*)")]
    public void GivenAUserExistsWithID(int userId)
    {
        var user = new UserRequests(userId, "test@example.com", "Test User", 1, true, DateTimeOffset.Now, "DNI", "12345678", "Address", "123456789");
        _userRepository.GetOneAsync(Arg.Is<UserSearchCriteria>(c => c.UserId == userId)).Returns(Task.FromResult<UserRequests?>(user));
    }

    [Given(@"a user with ID (.*) does not exist")]
    public void GivenAUserWithIDDoesNotExist(int userId)
    {
        _userRepository.GetOneAsync(Arg.Is<UserSearchCriteria>(c => c.UserId == userId)).Returns(Task.FromResult<UserRequests?>(null));
    }

    [When(@"I request the user with ID (.*)")]
    public async Task WhenIRequestTheUserWithID(int userId)
    {
        _getUserResult = await _userService.GetUserAsync(new UserSearchCriteria(userId, null, null));
    }

    [When(@"I update the user with ID (.*) with new info:")]
    public async Task WhenIUpdateTheUserWithIDWithNewInfo(int userId, DataTable table)
    {
        var row = table.Rows[0];
        var updateInfo = new UserUpdateInfo(
            Email: row.ContainsKey("Email") ? row["Email"] : null,
            FullName: row.ContainsKey("FullName") ? row["FullName"] : null
        );

        _updateUserResult = await _userService.UpdateUser(userId, updateInfo);
    }

    [Then(@"the result should be successful")]
    public void ThenTheResultShouldBeSuccessful()
    {
        Assert.That(_getUserResult?.IsSuccess, Is.True);
    }

    [Then(@"the user email should be ""(.*)""")]
    public void ThenTheUserEmailShouldBe(string email)
    {
        Assert.That(_getUserResult?.Value?.Email, Is.EqualTo(email));
    }

    [Then(@"the result should be a failure")]
    public void ThenTheResultShouldBeAFailure()
    {
        Assert.That(_getUserResult?.IsSuccess, Is.False);
    }

    [Then(@"the update result should be successful")]
    public void ThenTheUpdateResultShouldBeSuccessful()
    {
        Assert.That(_updateUserResult?.IsSuccess, Is.True);
    }

    [Then(@"the update result should be a failure")]
    public void ThenTheUpdateResultShouldBeAFailure()
    {
        Assert.That(_updateUserResult?.IsSuccess, Is.False);
    }

    [Then(@"the error message should be ""(.*)""")]
    public void ThenTheErrorMessageShouldBe(string errorMessage)
    {
        var actualError = _getUserResult != null ? _getUserResult.Error : _updateUserResult?.Error;
        Assert.That(actualError, Is.EqualTo(errorMessage));
    }

    [Then(@"the user update should be persisted")]
    public void ThenTheUserUpdateShouldBePersisted()
    {
        _userRepository.Received(1).UpdateUser(Arg.Any<int>(), Arg.Any<UserUpdateInfo>());
    }
}
