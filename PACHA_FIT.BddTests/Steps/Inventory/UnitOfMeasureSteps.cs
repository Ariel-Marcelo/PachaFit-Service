using Reqnroll;
using PACHA_FIT.Core.Application.Inventory;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class UnitOfMeasureSteps
{
    private readonly UnitOfMeasures _unitService = new UnitOfMeasures();
    private readonly ScenarioContext _scenarioContext;
    private decimal _currentFactor;
    private decimal _conversionResult;
    private decimal _quantity;
    private string _fromUnit = string.Empty;
    private bool _conversionFailed;

    public UnitOfMeasureSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)""")]
    public void GivenTheUnitOfMeasureWithAbbreviation(string name, string abbreviation)
    {
    }

    [When(@"I check the conversion factor for ""([^""]*)""")]
    public void WhenICheckTheConversionFactorFor(string abbreviation)
    {
        _currentFactor = _unitService.GetConversionFactor(abbreviation);
    }

    [Then(@"the factor should be (.*) relative to (.*)")]
    public void ThenTheFactorShouldBeRelativeTo(decimal expectedFactor, string baseUnit)
    {
        Assert.That(_currentFactor, Is.EqualTo(expectedFactor));
    }

    [Given(@"a quantity of (.*) ""([^""]*)""")]
    public void GivenAQuantityOf(decimal quantity, string unit)
    {
        _quantity = quantity;
        _fromUnit = unit;
    }

    [When(@"I convert the quantity to ""([^""]*)""")]
    public void WhenIConvertTheQuantityTo(string toUnit)
    {
        _conversionResult = _unitService.Convert(_quantity, _fromUnit, toUnit);
    }

    [When(@"I try to convert the quantity to ""([^""]*)""")]
    public void WhenITryToConvertTheQuantityTo(string toUnit)
    {
        try 
        {
            _conversionResult = _unitService.Convert(_quantity, _fromUnit, toUnit);
            _conversionFailed = false;
        }
        catch (InvalidOperationException ex)
        {
            _conversionFailed = true;
            _scenarioContext["ErrorMessage"] = ex.Message;
        }
    }

    [Then(@"the result should be (.*)")]
    public void ThenTheResultShouldBe(decimal expectedResult)
    {
        Assert.That(_conversionResult, Is.EqualTo(expectedResult));
    }

    [Then(@"the result should be (.*) with a precision of (.*) decimal places")]
    public void ThenTheResultShouldBeWithAPrecisionOfDecimalPlaces(decimal expectedResult, int precision)
    {
        var multiplier = (decimal)Math.Pow(10, precision);
        var actualRounded = Math.Round(_conversionResult * multiplier) / multiplier;
        var expectedRounded = Math.Round(expectedResult * multiplier) / multiplier;
        
        Assert.That(actualRounded, Is.EqualTo(expectedRounded));
    }

    [Then(@"the conversion should fail")]
    public void ThenTheConversionShouldFail()
    {
        Assert.That(_conversionFailed, Is.True);
    }
}
