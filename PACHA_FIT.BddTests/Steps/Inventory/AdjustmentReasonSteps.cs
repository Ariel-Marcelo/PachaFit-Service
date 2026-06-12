using Reqnroll;
using PACHA_FIT.Core.Application.Inventory;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class AdjustmentReasonSteps
{
    private readonly AdjustmentReasons _service = new AdjustmentReasons();
    private bool _isValid;
    private IEnumerable<string>? _allReasons;

    [When(@"I check if ""([^""]*)"" is a valid predefined reason")]
    public void WhenICheckIfIsAValidPredefinedReason(string reason)
    {
        _isValid = _service.IsValidReason(reason);
    }

    [Then(@"the result should be valid")]
    public void ThenTheResultShouldBeValid()
    {
        Assert.That(_isValid, Is.True);
    }

    [Then(@"the result should be invalid")]
    public void ThenTheResultShouldBeInvalid()
    {
        Assert.That(_isValid, Is.False);
    }

    [When(@"I request all predefined adjustment reasons")]
    public void WhenIRequestAllPredefinedAdjustmentReasons()
    {
        _allReasons = _service.GetAllReasons();
    }

    [Then(@"the list should contain exactly:")]
    public void ThenTheListShouldContainExactly(DataTable table)
    {
        var expectedReasons = table.Rows.Select(r => r["Name"]).ToList();
        Assert.That(_allReasons, Is.EquivalentTo(expectedReasons));
    }
}
