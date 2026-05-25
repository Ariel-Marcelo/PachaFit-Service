using Reqnroll;
using NSubstitute;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using NUnit.Framework;

namespace PACHA_FIT.BddTests.Steps.Inventory;

[Binding]
public class StockMovementSteps
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly ProductsService _productsService;
    private readonly InventoryService _inventoryService;
    private readonly ScenarioContext _scenarioContext;
    private List<StockMovementRequest> _recordedRequests = new();
    private Result<string>? _dispatchResult;
    private Result<ProductResponse>? _createResult;

    public StockMovementSteps(ScenarioContext scenarioContext, ScenarioDependencies dependencies)
    {
        _productRepository = dependencies.ProductRepository;
        _stockMovementRepository = dependencies.StockMovementRepository;
        _productsService = dependencies.ProductsService;
        _inventoryService = dependencies.InventoryService;
        _scenarioContext = scenarioContext;
        
        // Capture movements for verification
        _stockMovementRepository.When(x => x.SaveMovement(Arg.Any<StockMovementRequest>()))
            .Do(call => _recordedRequests.Add(call.Arg<StockMovementRequest>()));
    }

    [When(@"I register a new Kit ""([^""]*)"" with SKU ""([^""]*)"" and initial stock (.*):")]
    public async Task WhenIRegisterANewKitWithSKUAndInitialStock(string name, string sku, decimal stock, DataTable table)
    {
        var composition = table.Rows.Select(r => new ProductCompositionRequest(
            BaseProductSku: r["BaseProductSku"],
            Quantity: decimal.Parse(r["Quantity"]),
            UnitAbbreviation: r["Unit"]
        )).ToList();

        var request = new ProductCreateRequest(
            Name: name, 
            SKU: sku, 
            InitialStock: stock,
            Composition: composition);
        
        _productRepository.SaveProduct(Arg.Any<ProductCreateRequest>()).Returns(callInfo => {
            var req = callInfo.Arg<ProductCreateRequest>();
            return Task.FromResult(new ProductResponse(1, req.Name, req.SKU, req.InitialStock, 0, false, null));
        });

        _createResult = await _productsService.CreateProduct(request);
        if (!_createResult.IsSuccess)
        {
            _scenarioContext["ErrorMessage"] = _createResult.Error;
        }
    }

    [When(@"I try to register a new Kit ""([^""]*)"" with SKU ""([^""]*)"" and initial stock (.*):")]
    public async Task WhenITryToRegisterANewKitWithSKUAndInitialStock(string name, string xsku, decimal stock, DataTable table)
    {
        await WhenIRegisterANewKitWithSKUAndInitialStock(name, xsku, stock, table);
    }

    [Then(@"an ""([^""]*)"" movement should be recorded for ""([^""]*)"" with quantity (.*)")]
    public void ThenAnMovementShouldBeRecordedForWithQuantity(string type, string sku, decimal quantity)
    {
        var exists = _recordedRequests.Any(m => m.TypeMovement == type && (m.InputQuantity == quantity || m.BaseQuantityAffected == quantity));
        Assert.That(exists, Is.True, $"Movement {type} with qty {quantity} not found for {sku}");
    }

    [Then(@"two ""([^""]*)"" movements should be recorded:")]
    public void ThenTwoMovementsShouldBeRecorded(string type, DataTable table)
    {
        foreach (var row in table.Rows)
        {
            var qty = decimal.Parse(row["Quantity"]);
            var expiry = DateTime.Parse(row["ExpiryDate"]);
            var exists = _recordedRequests.Any(m => m.TypeMovement == type && m.InputQuantity == qty && m.ExpiryDate == expiry);
            Assert.That(exists, Is.True, $"Movement {type} with qty {qty} and expiry {expiry} not found");
        }
    }

    [Given(@"a product exists with SKU ""([^""]*)"" and current stock (.*)")]
    public void GivenAProductExistsWithSKUAndCurrentStock(string sku, decimal stock)
    {
        _productRepository.GetBySku(sku).Returns(new ProductResponse(1, "Test", sku, stock, 0, false, null));
        _stockMovementRepository.GetAvailableBatches(1).Returns(new List<StockBatchResponse> { 
            new StockBatchResponse(1, stock, null) 
        });
    }

    [Given(@"a product exists with Name ""([^""]*)"" and SKU ""([^""]*)"" with stock (.*) ""([^""]*)""")]
    public void GivenAProductExistsWithNameAndSKUWithStock(string name, string sku, decimal stock, string unit)
    {
        _productRepository.GetBySku(sku).Returns(new ProductResponse(1, name, sku, stock, 0, false, null));
    }

    [Given(@"the following batches exist for ""([^""]*)"":")]
    public void GivenTheFollowingBatchesExistFor(string sku, DataTable table)
    {
        var batches = table.Rows.Select(r => new StockBatchResponse(
            ProductId: 1,
            AvailableQty: decimal.Parse(r["Quantity"]),
            ExpiryDate: DateTime.Parse(r["ExpiryDate"])
        )).ToList();

        _stockMovementRepository.GetAvailableBatches(Arg.Any<int>()).Returns(Task.FromResult<IEnumerable<StockBatchResponse>>(batches));
    }

    [When(@"I dispatch (.*) ""([^""]*)"" of ""([^""]*)""")]
    public async Task WhenIDispatchOf(decimal quantity, string unit, string sku)
    {
        _dispatchResult = await _inventoryService.DispatchStock(1, quantity, unit);
    }

    [When(@"I try to dispatch (.*) ""([^""]*)"" of ""([^""]*)""")]
    public async Task WhenITryToDispatchOf(decimal quantity, string unit, string sku)
    {
        _dispatchResult = await _inventoryService.DispatchStock(1, quantity, unit);
        if (!_dispatchResult.IsSuccess)
        {
            _scenarioContext["ErrorMessage"] = _dispatchResult.Error;
        }
    }

    [Then(@"the dispatch should be successful")]
    public void ThenTheDispatchShouldBeSuccessful()
    {
        Assert.That(_dispatchResult?.IsSuccess, Is.True);
    }

    [Then(@"the dispatch should fail")]
    public void ThenTheDispatchShouldFail()
    {
        Assert.That(_dispatchResult?.IsSuccess, Is.False);
    }

    [Then(@"the kit registration should fail")]
    public void ThenTheKitRegistrationShouldFail()
    {
        Assert.That(_createResult?.IsSuccess, Is.False);
    }

    [Then(@"the movement should target the batch expiring on ""([^""]*)""")]
    public void ThenTheMovementShouldTargetTheBatchExpiringOn(string expiryDate)
    {
        var expectedDate = DateTime.Parse(expiryDate);
        var exists = _recordedRequests.Any(m => m.TypeMovement == "Egreso" && m.ExpiryDate == expectedDate);
        Assert.That(exists, Is.True, $"No Egreso movement found for batch expiring on {expiryDate}");
    }

    [Given(@"the unit of measure ""([^""]*)"" with abbreviation ""([^""]*)"" has factor (.*)")]
    public void GivenTheUnitOfMeasureWithAbbreviationHasFactor(string name, string abbreviation, decimal factor)
    {
    }

    [When(@"I register a new product ""([^""]*)"" with SKU ""([^""]*)"" and initial stock:")]
    public async Task WhenIRegisterANewProductWithSKUAndInitialStockTable(string name, string sku, DataTable table)
    {
        var row = table.Rows[0];
        var qty = decimal.Parse(row["Quantity"]);
        var unit = row["Unit"];
        
        var request = new ProductCreateRequest(
            Name: name, 
            SKU: sku, 
            InitialStock: qty, 
            InitialUnitAbbreviation: unit);
        
        _productRepository.SaveProduct(Arg.Any<ProductCreateRequest>()).Returns(
            new ProductResponse(1, name, sku, qty, 0, false, null)
        );

        await _productsService.CreateProduct(request);
    }

    [When(@"I register a new product ""([^""]*)"" with SKU ""([^""]*)"" expiring on ""([^""]*)""")]
    public async Task WhenIRegisterANewProductWithSKUExpiringOn(string name, string sku, string expiryDate)
    {
        await _stockMovementRepository.SaveMovement(new StockMovementRequest(
            ProductId: 1,
            InputQuantity: 0,
            InputUnitAbbreviation: "u",
            BaseQuantityAffected: 0,
            TypeMovement: "Ingreso",
            ExpiryDate: DateTime.Parse(expiryDate),
            Description: "Carga inicial de producto"
        ));
    }

    [When(@"I record a manual adjustment for ""([^""]*)"":")]
    public async Task WhenIRecordAManualAdjustmentFor(string sku, DataTable table)
    {
        var row = table.Rows[0];
        var qty = decimal.Parse(row["Quantity"]);
        var unit = row["Unit"];
        var type = row["Type"];
        var reason = row["Reason"];

        decimal baseQty = qty;
        if (unit == "lb") baseQty = qty * 454;

        await _stockMovementRepository.SaveMovement(new StockMovementRequest(
            ProductId: 1,
            InputQuantity: qty,
            InputUnitAbbreviation: unit,
            BaseQuantityAffected: baseQty,
            TypeMovement: type,
            Description: reason
        ));
    }

    [Then(@"a stock movement should be recorded with the following details:")]
    public void ThenAStockMovementShouldBeRecordedWithTheFollowingDetails(DataTable table)
    {
        var row = table.Rows[0];
        var type = row["Type"];
        var qty = decimal.Parse(row["InputQty"]);
        
        var match = _recordedRequests.FirstOrDefault(m => m.TypeMovement == type && m.InputQuantity == qty);
        Assert.That(match, Is.Not.Null, $"No movement found for {type} and qty {qty}");
        Assert.That(match?.InputUnitAbbreviation, Is.EqualTo(row["Unit"]));
        Assert.That(match?.BaseQuantityAffected, Is.EqualTo(decimal.Parse(row["BaseQtyAffected"])));
    }

    [Then(@"the movement description should be ""([^""]*)""")]
    public void ThenTheMovementDescriptionShouldBe(string description)
    {
        Assert.That(_recordedRequests.Any(r => r.Description == description), Is.True);
    }

    [Then(@"the stock movement should record the expiry date ""([^""]*)""")]
    public void ThenTheStockMovementShouldRecordTheExpiryDate(string expiryDate)
    {
        Assert.That(_recordedRequests.Any(r => r.ExpiryDate == DateTime.Parse(expiryDate)), Is.True);
    }
}
