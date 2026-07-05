using Reqnroll;
using NSubstitute;
using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.BddTests.Steps.User;

[Binding]
public class AuthServiceSteps
{
    private readonly ICredentialService _credentialService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly AuthService _authService;
    
    private Result<AuthSession>? _loginResult;
    private Result<string>? _signUpResult;

    public AuthServiceSteps(ScenarioDependencies dependencies)
    {
        _credentialService = dependencies.CredentialService;
        _userRepository = dependencies.UserRepository;
        _passwordService = dependencies.PasswordService;
        _authService = dependencies.AuthService;
    }

    [Given(@"a user exists with email ""([^""]*)"" and password ""([^""]*)""")]
    public void GivenAUserExistsWithEmailAndPassword(string email, string password)
    {
        var hashedPwd = "hashed_" + password;
        var user = new InternalUserResponse(1, email, "Test User", hashedPwd, "User");
        _userRepository.GetInternalUserAsync(email).Returns(Task.FromResult<InternalUserResponse?>(user));
        _passwordService.VerifyPassword(password, hashedPwd).Returns(true);
    }

    [Given(@"a user with email ""([^""]*)"" does not exist")]
    public void GivenAUserWithEmailDoesNotExist(string email)
    {
        _userRepository.GetInternalUserAsync(email).Returns(Task.FromResult<InternalUserResponse?>(null));
    }

    [Given(@"a user exists with email ""([^""]*)""$")]
    public void GivenAUserExistsWithEmail(string email)
    {
        var user = new InternalUserResponse(1, email, "Test User", "some_hash", "User");
        _userRepository.GetInternalUserAsync(email).Returns(Task.FromResult<InternalUserResponse?>(user));
    }

    [When(@"I login with email ""(.*)"" and password ""(.*)""")]
    public async Task WhenILoginWithEmailAndPassword(string email, string password)
    {
        _loginResult = await _authService.LoginUser(new AuthCredentials(email, password));
    }

    [When(@"I sign up with:")]
    public async Task WhenISignUpWith(DataTable table)
    {
        var row = table.Rows[0];
        var registration = new NewUserRegistration(
            Email: row["Email"],
            Password: row["Password"],
            FullName: row["FullName"]
        );
        _signUpResult = await _authService.SignUp(registration);
    }

    [Then(@"the login result should be successful")]
    public void ThenTheLoginResultShouldBeSuccessful()
    {
        Assert.That(_loginResult?.IsSuccess, Is.True, $"Login should be successful. Error: {_loginResult?.Error}");
    }

    [Then(@"the session should have email ""(.*)""")]
    public void ThenTheSessionShouldHaveEmail(string email)
    {
        Assert.That(_loginResult?.Value?.Email, Is.EqualTo(email));
    }

    [Then(@"the login result should be a failure")]
    public void ThenTheLoginResultShouldBeAFailure()
    {
        Assert.That(_loginResult?.IsSuccess, Is.False);
    }

    [Then(@"the login error message should be ""(.*)""")]
    public void ThenTheLoginErrorMessageShouldBe(string errorMessage)
    {
        Assert.That(_loginResult?.Error, Is.EqualTo(errorMessage));
    }

    [Then(@"the sign up result should be successful")]
    public void ThenTheSignUpResultShouldBeSuccessful()
    {
        Assert.That(_signUpResult?.IsSuccess, Is.True, $"Sign up should be successful. Error: {_signUpResult?.Error}");
    }

    [Then(@"the sign up result should be a failure")]
    public void ThenTheSignUpResultShouldBeAFailure()
    {
        Assert.That(_signUpResult?.IsSuccess, Is.False);
    }

    [Then(@"the sign up error message should be ""(.*)""")]
    public void ThenTheSignUpErrorMessageShouldBe(string errorMessage)
    {
        Assert.That(_signUpResult?.Error, Is.EqualTo(errorMessage));
    }

    [Then(@"the user should be saved")]
    public void ThenTheUserShouldBeSaved()
    {
        _userRepository.Received(1).SaveUser(Arg.Any<PACHA_FIT.Core.Domain.User.User>());
    }
}
