using Reqnroll;
using NSubstitute;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class UnitOfMeasureSteps
{
    private readonly UnitOfMeasureService _unitService;
    private readonly IUnitOfMeasureRepository _repository;
    private readonly ScenarioContext _scenarioContext;
    private Result<decimal>? _currentFactorResult;
    private Result<decimal>? _conversionResult;
    private Result<UnitOfMeasureGroupedDto>? _activeUnitsResult;
    private decimal _quantity;
    private string _fromUnit = string.Empty;
    private bool _conversionFailed;
    private List<UnitOfMeasureInfo> _mockUnits = new();

    public UnitOfMeasureSteps(ScenarioContext scenarioContext, ScenarioDependencies dependencies)
    {
        _unitService = dependencies.UnitOfMeasureService;
        _repository = dependencies.UnitOfMeasureRepository;
        _scenarioContext = scenarioContext;
        
        // Setup repository to return only active mock units from the shared list
        _repository.GetAllActiveAsync().Returns(callInfo => 
        {
            var activeUnits = _mockUnits.Where(u => u.IsActive).ToList();
            return Task.FromResult<IEnumerable<UnitOfMeasureInfo>>(activeUnits);
        });
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

        _mockUnits.Add(new UnitOfMeasureInfo(name, abbreviation, category, factor, true));
        _unitService.ClearCache();
    }

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)"" is inactive")]
    public void GivenTheUnitOfMeasureWithAbbreviationIsInactive(string name, string abbreviation)
    {
        _mockUnits.Add(new UnitOfMeasureInfo(name, abbreviation, "masa", 1.0m, false));
        _unitService.ClearCache();
    }

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)"" has factor (.*)")]
    public void GivenTheUnitOfMeasureWithAbbreviationHasFactor(string name, string abbreviation, decimal factor)
    {
        _mockUnits.Add(new UnitOfMeasureInfo(name, abbreviation, "masa", factor, true));
        _unitService.ClearCache();
    }

    [When(@"I check the conversion factor for ""([^""]*)""")]
    public async Task WhenICheckTheConversionFactorFor(string abbreviation)
    {
        _currentFactorResult = await _unitService.GetConversionFactor(abbreviation);
    }

    [When(@"I request all active units of measure")]
    public async Task WhenIRequestAllActiveUnitsOfMeasure()
    {
        _activeUnitsResult = await _unitService.GetAllActiveUnitsAsync();
    }

    [Then(@"the list should contain ""([^""]*)""")]
    public void ThenTheListShouldContain(string abbreviation)
    {
        Assert.That(_activeUnitsResult?.IsSuccess, Is.True);
        var dto = _activeUnitsResult!.Value!;
        var exists = dto.MassUnits.Any(u => u.Abbreviation == abbreviation) ||
                     dto.VolumeUnits.Any(u => u.Abbreviation == abbreviation) ||
                     dto.DiscreteUnits.Any(u => u.Abbreviation == abbreviation);
        Assert.That(exists, Is.True);
    }

    [Then(@"the list should not contain ""([^""]*)""")]
    public void ThenTheListShouldNotContain(string abbreviation)
    {
        Assert.That(_activeUnitsResult?.IsSuccess, Is.True);
        var dto = _activeUnitsResult!.Value!;
        var exists = dto.MassUnits.Any(u => u.Abbreviation == abbreviation) ||
                     dto.VolumeUnits.Any(u => u.Abbreviation == abbreviation) ||
                     dto.DiscreteUnits.Any(u => u.Abbreviation == abbreviation);
        Assert.That(exists, Is.False);
    }

    [Then(@"the factor should be (.*) relative to (.*)")]
    public void ThenTheFactorShouldBeRelativeTo(decimal expectedFactor, string baseUnit)
    {
        Assert.That(_currentFactorResult?.IsSuccess, Is.True);
        Assert.That(_currentFactorResult?.Value, Is.EqualTo(expectedFactor));
    }

    [Given(@"a quantity of (.*) ""([^""]*)""")]
    public void GivenAQuantityOf(decimal quantity, string unit)
    {
        _quantity = quantity;
        _fromUnit = unit;
        
        // Add basic units if not already present for conversion tests
        bool added = false;
        if (!_mockUnits.Any(u => u.Abbreviation == "g")) { _mockUnits.Add(new UnitOfMeasureInfo("Gramos", "g", "masa", 1.0m, true)); added = true; }
        if (!_mockUnits.Any(u => u.Abbreviation == "lb")) { _mockUnits.Add(new UnitOfMeasureInfo("Libra", "lb", "masa", 454.0m, true)); added = true; }
        if (!_mockUnits.Any(u => u.Abbreviation == "qq")) { _mockUnits.Add(new UnitOfMeasureInfo("Quintal", "qq", "masa", 45400.0m, true)); added = true; }
        if (!_mockUnits.Any(u => u.Abbreviation == "kg")) { _mockUnits.Add(new UnitOfMeasureInfo("Kilo", "kg", "masa", 1000.0m, true)); added = true; }
        if (!_mockUnits.Any(u => u.Abbreviation == "ml")) { _mockUnits.Add(new UnitOfMeasureInfo("Mililitro", "ml", "volumen", 1.0m, true)); added = true; }
        
        if (added) _unitService.ClearCache();
    }

    [When(@"I convert the quantity to ""([^""]*)""")]
    public async Task WhenIConvertTheQuantityTo(string toUnit)
    {
        _conversionResult = await _unitService.Convert(_quantity, _fromUnit, toUnit);
    }

    [When(@"I try to convert the quantity to ""([^""]*)""")]
    public async Task WhenITryToConvertTheQuantityTo(string toUnit)
    {
        _conversionResult = await _unitService.Convert(_quantity, _fromUnit, toUnit);
        if (!_conversionResult.IsSuccess)
        {
            _conversionFailed = true;
            _scenarioContext["ErrorMessage"] = _conversionResult.Error;
        }
        else 
        {
            _conversionFailed = false;
        }
    }

    [Then(@"the result should be (.*)")]
    public void ThenTheResultShouldBe(decimal expectedResult)
    {
        Assert.That(_conversionResult?.IsSuccess, Is.True);
        Assert.That(_conversionResult?.Value, Is.EqualTo(expectedResult));
    }

    [Then(@"the result should be (.*) with a precision of (.*) decimal places")]
    public void ThenTheResultShouldBeWithAPrecisionOfDecimalPlaces(decimal expectedResult, int precision)
    {
        Assert.That(_conversionResult?.IsSuccess, Is.True);
        var multiplier = (decimal)Math.Pow(10, precision);
        var actualRounded = Math.Round(_conversionResult!.Value * multiplier) / multiplier;
        var expectedRounded = Math.Round(expectedResult * multiplier) / multiplier;
        
        Assert.That(actualRounded, Is.EqualTo(expectedRounded));
    }

    [Then(@"the conversion should fail")]
    public void ThenTheConversionShouldFail()
    {
        Assert.That(_conversionFailed, Is.True);
    }
}
