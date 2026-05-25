using Reqnroll;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps;

[Binding]
public class SharedSteps
{
    private readonly ScenarioContext _scenarioContext;

    public SharedSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Then(@"the error message should be ""([^""]*)""")]
    public void ThenTheErrorMessageShouldBe(string expectedMessage)
    {
        if (!_scenarioContext.TryGetValue("ErrorMessage", out string actualError))
        {
            Assert.Fail("Error message not found in ScenarioContext");
        }
        Assert.That(actualError, Is.EqualTo(expectedMessage));
    }

    [Then(@"the error message should include ""([^""]*)""")]
    public void ThenTheErrorMessageShouldInclude(string expectedPart)
    {
        if (!_scenarioContext.TryGetValue("ErrorMessage", out string actualError))
        {
            Assert.Fail("Error message not found in ScenarioContext");
        }
        Assert.That(actualError, Does.Contain(expectedPart));
    }
}
