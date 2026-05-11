using Reqnroll;
using PACHA_FIT.Core.Application.Inventory;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class UnitOfMeasureSteps
{
    private readonly UnitOfMeasures _unitService = new UnitOfMeasures();
    private decimal _currentFactor;
    private decimal _conversionResult;
    private decimal _quantity;
    private string _fromUnit = string.Empty;

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)""")]
    public void GivenTheUnitOfMeasureWithAbbreviation(string name, string abbreviation)
    {
        // This step might be used to ensure the unit is "known" or registered in the standard
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

    [Then(@"the result should be (.*)")]
    public void ThenTheResultShouldBe(decimal expectedResult)
    {
        Assert.That(_conversionResult, Is.EqualTo(expectedResult));
    }
}
