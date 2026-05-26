using Reqnroll;
using NSubstitute;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Infrastructure.Persistence.Entities;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class UnitOfMeasureSteps
{
    private readonly UnitOfMeasureService _unitService;
    private readonly IUnitOfMeasureRepository _repository;
    private readonly ScenarioContext _scenarioContext;
    private decimal _currentFactor;
    private decimal _conversionResult;
    private decimal _quantity;
    private string _fromUnit = string.Empty;
    private bool _conversionFailed;
    private List<UnitOfMeasure> _mockUnits = new();

    public UnitOfMeasureSteps(ScenarioContext scenarioContext, ScenarioDependencies dependencies)
    {
        _unitService = dependencies.UnitOfMeasureService;
        _repository = dependencies.UnitOfMeasureRepository;
        _scenarioContext = scenarioContext;
        
        // Setup repository to return our mock list
        _repository.GetAllAsync().Returns(Task.FromResult<IEnumerable<UnitOfMeasure>>(_mockUnits));
    }

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)""")]
    public void GivenTheUnitOfMeasureWithAbbreviation(string name, string abbreviation)
    {
        var category = "masa";
        if (abbreviation == "ml" || abbreviation == "L") category = "volumen";
        if (abbreviation == "u") category = "unidades";

        decimal factor = 1.0m;
        if (abbreviation == "lb") factor = 454.0m;
        if (abbreviation == "@") factor = 11350.0m;
        if (abbreviation == "qq") factor = 45400.0m;
        if (abbreviation == "kg") factor = 1000.0m;
        if (abbreviation == "L") factor = 1000.0m;

        _mockUnits.Add(new UnitOfMeasure { Name = name, Abbreviation = abbreviation, Category = category, ConversionFactor = factor });
    }

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)"" has factor (.*)")]
    public void GivenTheUnitOfMeasureWithAbbreviationHasFactor(string name, string abbreviation, decimal factor)
    {
        _mockUnits.Add(new UnitOfMeasure { Name = name, Abbreviation = abbreviation, Category = "masa", ConversionFactor = factor });
    }

    [When(@"I check the conversion factor for ""([^""]*)""")]
    public async Task WhenICheckTheConversionFactorFor(string abbreviation)
    {
        _currentFactor = await _unitService.GetConversionFactor(abbreviation);
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
        
        // Add basic units if not already present for conversion tests
        if (!_mockUnits.Any(u => u.Abbreviation == "g")) _mockUnits.Add(new UnitOfMeasure { Abbreviation = "g", Category = "masa", ConversionFactor = 1.0m });
        if (!_mockUnits.Any(u => u.Abbreviation == "lb")) _mockUnits.Add(new UnitOfMeasure { Abbreviation = "lb", Category = "masa", ConversionFactor = 454.0m });
        if (!_mockUnits.Any(u => u.Abbreviation == "qq")) _mockUnits.Add(new UnitOfMeasure { Abbreviation = "qq", Category = "masa", ConversionFactor = 45400.0m });
        if (!_mockUnits.Any(u => u.Abbreviation == "kg")) _mockUnits.Add(new UnitOfMeasure { Abbreviation = "kg", Category = "masa", ConversionFactor = 1000.0m });
        if (!_mockUnits.Any(u => u.Abbreviation == "ml")) _mockUnits.Add(new UnitOfMeasure { Abbreviation = "ml", Category = "volumen", ConversionFactor = 1.0m });
    }

    [When(@"I convert the quantity to ""([^""]*)""")]
    public async Task WhenIConvertTheQuantityTo(string toUnit)
    {
        _conversionResult = await _unitService.Convert(_quantity, _fromUnit, toUnit);
    }

    [When(@"I try to convert the quantity to ""([^""]*)""")]
    public async Task WhenITryToConvertTheQuantityTo(string toUnit)
    {
        try 
        {
            _conversionResult = await _unitService.Convert(_quantity, _fromUnit, toUnit);
            _conversionFailed = false;
        }
        catch (Exception ex)
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
