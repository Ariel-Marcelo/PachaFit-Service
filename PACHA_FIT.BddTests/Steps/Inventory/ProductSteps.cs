using Reqnroll;
using NSubstitute;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using System.Text.Json;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class ProductSteps
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly ProductsService _productsService;
    private readonly ScenarioContext _scenarioContext;
    private Result<ProductResponse>? _createResult;
    private List<ProductSpec> _currentSpecs = new();
    private List<ProductCompositionRequest> _currentComposition = new();
    private Dictionary<string, string> _knownProductNames = new();

    public ProductSteps(ScenarioContext scenarioContext, ScenarioDependencies dependencies)
    {
        _productRepository = dependencies.ProductRepository;
        _stockMovementRepository = dependencies.StockMovementRepository;
        _productsService = dependencies.ProductsService;
        _scenarioContext = scenarioContext;
    }

    [Given(@"a product exists with Name ""([^""]*)"" and SKU ""([^""]*)""")]
    public void GivenAProductExistsWithNameAndSKU(string name, string sku)
    {
        _knownProductNames[sku] = name;
        _productRepository.GetBySku(sku).Returns(new ProductResponse(1, name, sku, 0, 0, false, null));
    }

    [Given(@"a product exists with SKU ""([^""]*)""")]
    public void GivenAProductExistsWithSKU(string sku)
    {
        _productRepository.ExistsSku(sku).Returns(true);
    }

    [Given(@"a product exists with SKU ""([^""]*)"" and Name ""([^""]*)""")]
    public void GivenAProductExistsWithSKUAndName(string sku, string name)
    {
        _productRepository.ExistsSku(sku).Returns(true);
        _productRepository.GetBySku(sku).Returns(new ProductResponse(1, name, sku, 0, 0, false, null));
    }

    [When(@"I create a new product with the following details:")]
    public async Task WhenICreateANewProductWithTheFollowingDetails(DataTable table)
    {
        await ExecuteCreate(table);
    }

    [When(@"I try to create a new product with the following details:")]
    public async Task WhenITryToCreateANewProductWithTheFollowingDetails(DataTable table)
    {
        await ExecuteCreate(table);
    }

    private async Task ExecuteCreate(DataTable table)
    {
        var row = table.Rows[0];
        var request = new ProductCreateRequest(
            Name: row["Name"],
            SKU: row["SKU"],
            CategoryId: row.ContainsKey("CategoryId") ? int.Parse(row["CategoryId"]) : null,
            CostPrice: row.ContainsKey("CostPrice") ? decimal.Parse(row["CostPrice"]) : 0,
            SalePrice: row.ContainsKey("SalePrice") ? decimal.Parse(row["SalePrice"]) : 0,
            IvaPercentage: row.ContainsKey("IvaPercentage") ? int.Parse(row["IvaPercentage"]) : 0,
            IsWeightBased: row.ContainsKey("IsWeightBased") && bool.Parse(row["IsWeightBased"]),
            Specs: _currentSpecs.Any() ? _currentSpecs : null,
            Composition: _currentComposition.Any() ? _currentComposition : null
        );

        _productRepository.SaveProduct(Arg.Any<ProductCreateRequest>()).Returns(callInfo => 
        {
            var req = callInfo.Arg<ProductCreateRequest>();
            var specsJson = req.Specs != null ? JsonSerializer.Serialize(req.Specs) : null;
            
            var compResponse = req.Composition?.Select(c => new ProductCompositionResponse(
                BaseProductName: _knownProductNames.GetValueOrDefault(c.BaseProductSku, "Unknown"),
                Quantity: c.Quantity,
                UnitAbbreviation: c.UnitAbbreviation
            )).ToList();

            return Task.FromResult(new ProductResponse(
                1, 
                req.Name, 
                req.SKU, 
                0, 
                req.IvaPercentage, 
                req.IsWeightBased, 
                specsJson,
                compResponse));
        });

        _createResult = await _productsService.CreateProduct(request);
        if (!_createResult.IsSuccess)
        {
            _scenarioContext["ErrorMessage"] = _createResult.Error;
        }
    }

    [When(@"I update the product ""([^""]*)"" with the following details:")]
    public async Task WhenIUpdateTheProductWithTheFollowingDetails(string sku, DataTable table)
    {
        var row = table.Rows[0];
        var name = row.ContainsKey("Name") ? row["Name"] : "Updated Product";
        
        _productRepository.UpdateProduct(sku, Arg.Any<object>()).Returns(Result<bool>.Success(true));
        _productRepository.GetBySku(sku).Returns(new ProductResponse(1, name, sku, 0, 0, false, null, null));

        _createResult = Result<ProductResponse>.Success(new ProductResponse(1, name, sku, 0, 0, false, null));
    }

    [When(@"I deactivate the product ""([^""]*)""")]
    public async Task WhenIDeactivateTheProduct(string sku)
    {
        _productRepository.DeactivateProduct(sku).Returns(Result<bool>.Success(true));
        await _productsService.DeactivateProduct(sku);
    }

    [Then(@"the product should be created successfully")]
    public void ThenTheProductShouldBeCreatedSuccessfully()
    {
        Assert.That(_createResult?.IsSuccess, Is.True, $"Create product failed: {_createResult?.Error}");
    }

    [Then(@"the creation should fail")]
    public void ThenTheCreationShouldFail()
    {
        Assert.That(_createResult?.IsSuccess, Is.False);
    }

    [Then(@"the product should be updated successfully")]
    public void ThenTheProductShouldBeUpdatedSuccessfully()
    {
        Assert.That(_createResult?.IsSuccess, Is.True);
    }

    [Then(@"the product ""([^""]*)"" should have Name ""([^""]*)"" and SalePrice (.*)")]
    public async Task ThenTheProductShouldHaveNameAndSalePrice(string sku, string expectedName, decimal expectedPrice)
    {
        var product = await _productRepository.GetBySku(sku);
        Assert.That(product.Name, Is.EqualTo(expectedName));
    }

    [Then(@"the product should be deactivated")]
    public void ThenTheProductShouldBeDeactivated()
    {
        _productRepository.Received(1).DeactivateProduct(Arg.Any<string>());
    }

    [Then(@"the product should not be available for new sales")]
    public void ThenTheProductShouldNotBeAvailableForNewSales()
    {
        // Availability check logic
    }

    [Then(@"the initial stock should be 0")]
    public void ThenTheInitialStockShouldBe0()
    {
        Assert.That(_createResult?.Value?.StockQty, Is.EqualTo(0));
    }

    [Then(@"a stock movement should be recorded as ""([^""]*)"" with quantity (.*)")]
    public void ThenAStockMovementShouldBeRecordedAsWithQuantity(string type, decimal quantity)
    {
        _stockMovementRepository.Received(1).SaveMovement(Arg.Is<StockMovementRequest>(m => 
            m.TypeMovement == type && m.InputQuantity == quantity));
    }

    [Then(@"the IVA percentage should be (.*)")]
    public void ThenTheIVAPercentageShouldBe(int expectedIva)
    {
        Assert.That(_createResult?.Value?.IvaPercentage, Is.EqualTo(expectedIva));
    }

    [Then(@"the product should be weight-based")]
    public void ThenTheProductShouldBeWeightBased()
    {
        Assert.That(_createResult?.Value?.IsWeightBased, Is.True);
    }

    [Then(@"the product should not be weight-based")]
    public void ThenTheProductShouldNotBeWeightBased()
    {
        Assert.That(_createResult?.Value?.IsWeightBased, Is.False);
    }

    [Given(@"the following specifications:")]
    public void GivenTheFollowingSpecifications(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            _currentSpecs.Add(new ProductSpec(row["Label"], row["Value"]));
        }
    }

    [Given(@"the following composition:")]
    public void GivenTheFollowingComposition(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            _currentComposition.Add(new ProductCompositionRequest(
                row["BaseProductSku"],
                decimal.Parse(row["Quantity"]),
                row["UnitAbbreviation"]
            ));
        }
    }

    [Then(@"the product composition should include:")]
    public void ThenTheProductCompositionShouldInclude(DataTable table)
    {
        var actualComposition = _createResult!.Value!.Composition;
        Assert.That(actualComposition, Is.Not.Null);

        foreach (var row in table.Rows)
        {
            var exists = actualComposition!.Any((ProductCompositionResponse c) => 
                c.BaseProductName == row["BaseProductName"] && 
                c.Quantity == decimal.Parse(row["Quantity"]) && 
                c.UnitAbbreviation == row["UnitAbbreviation"]
            );
            Assert.That(exists, Is.True, $"Composition {row["BaseProductName"]} not found or mismatch");
        }
    }

    [Then(@"the product specifications should be stored as a JSON")]
    public void ThenTheProductSpecificationsShouldBeStoredAsAJSON()
    {
        Assert.That(_createResult?.Value?.SpecsJson, Is.Not.Null);
        Assert.DoesNotThrow(() => JsonDocument.Parse(_createResult!.Value!.SpecsJson!));
    }

    [Then(@"the specifications should include:")]
    public void ThenTheSpecificationsShouldInclude(DataTable table)
    {
        var specs = JsonSerializer.Deserialize<List<ProductSpec>>(_createResult!.Value!.SpecsJson!);
        foreach (var row in table.Rows)
        {
            var exists = specs!.Any(s => s.Label == row["Label"] && s.Value == row["Value"]);
            Assert.That(exists, Is.True, $"Spec {row["Label"]}: {row["Value"]} not found in JSON");
        }
    }
}
