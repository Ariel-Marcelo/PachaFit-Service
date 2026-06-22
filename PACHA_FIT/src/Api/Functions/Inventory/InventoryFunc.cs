using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api.Mappers;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Infrastructure.Nswag;

namespace PACHA_FIT.Api.Functions.Inventory;

public class InventoryFunc : InventoryControllerBase
{
    private readonly ILogger<InventoryFunc> _logger;
    private readonly UnitOfMeasureService _unitOfMeasureService;
    private readonly ProductsService _productsService;
    private readonly InventoryService _inventoryService;
    private readonly AdjustmentReasons _adjustmentReasons;

    public InventoryFunc(
        ILogger<InventoryFunc> logger, 
        UnitOfMeasureService unitOfMeasureService, 
        ProductsService productsService, 
        InventoryService inventoryService, 
        AdjustmentReasons adjustmentReasons)
    {
        _logger = logger;
        _unitOfMeasureService = unitOfMeasureService;
        _productsService = productsService;
        _inventoryService = inventoryService;
        _adjustmentReasons = adjustmentReasons;
    }

    [Function("Inventory_GetAdjustmentReasons")]
    public async Task<ICollection<string>> RunGetAdjustmentReasons(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/adjustment-reasons")] HttpRequest req)
    {
        return await this.GetAdjustmentReasons();
    }

    [Function("Inventory_GetUnits")]
    public async Task<ResultDtoOfUnitOfMeasureGroupedDto> RunGetUnits(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/units")] HttpRequest req)
    {
        return await this.GetUnits();
    }

    [Function("Inventory_ConvertUnit")]
    public async Task<ResultDtoOfDecimal> RunConvertUnit(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/units/convert")] HttpRequest req)
    {
        if (!double.TryParse(req.Query["quantity"], out double quantity)) return new ResultDtoOfDecimal { IsSuccess = false, Error = "Invalid quantity" };
        string fromUnit = req.Query["fromUnit"]!;
        string toUnit = req.Query["toUnit"]!;

        return await this.ConvertUnit(quantity, fromUnit, toUnit);
    }

    [Function("Inventory_CreateProduct")]
    public async Task<ResultDtoOfProductResponse> RunCreateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/products")] HttpRequest req)
    {
        var body = await req.ReadFromJsonAsync<PACHA_FIT.Infrastructure.Nswag.ProductCreateRequest>();
        if (body == null) return new ResultDtoOfProductResponse { IsSuccess = false, Error = "Invalid request body" };
        return await this.CreateProduct(body);
    }

    [Function("Inventory_DeactivateProduct")]
    public async Task<ResultDtoOfBoolean> RunDeactivateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/products/{sku}/deactivate")] HttpRequest req, string sku)
    {
        return await this.DeactivateProduct(sku);
    }

    [Function("Inventory_DispatchStock")]
    public async Task<ResultDtoOfString> RunDispatchStock(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/dispatch")] HttpRequest req)
    {
        var body = await req.ReadFromJsonAsync<DispatchStockRequest>();
        if (body == null) return new ResultDtoOfString { IsSuccess = false, Error = "Invalid request body" };
        return await this.DispatchStock(body);
    }

    // Abstract implementations

    public override Task<ICollection<string>> GetAdjustmentReasons(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ICollection<string>>(_adjustmentReasons.GetAllReasons().ToList());
    }

    public override async Task<ResultDtoOfUnitOfMeasureGroupedDto> GetUnits(CancellationToken cancellationToken = default)
    {
        var result = await _unitOfMeasureService.GetAllActiveUnitsAsync();
        var res = result.Value?.ToApi();
        return new ResultDtoOfUnitOfMeasureGroupedDto
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value?.ToApi(),
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    public override async Task<ResultDtoOfDecimal> ConvertUnit(double quantity, string fromUnit, string toUnit, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfMeasureService.Convert((decimal)quantity, fromUnit, toUnit);
        return new ResultDtoOfDecimal
        {
            IsSuccess = result.IsSuccess,
            Value = (double)result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    public override async Task<ResultDtoOfProductResponse> CreateProduct(PACHA_FIT.Infrastructure.Nswag.ProductCreateRequest body, CancellationToken cancellationToken = default)
    {
        var domainRequest = body.ToDomain();
        var result = await _productsService.CreateProduct(domainRequest);
        return new ResultDtoOfProductResponse
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value?.ToApi(),
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    public override async Task<ResultDtoOfBoolean> DeactivateProduct(string sku, CancellationToken cancellationToken = default)
    {
        var result = await _productsService.DeactivateProduct(sku);
        return new ResultDtoOfBoolean
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    public override async Task<ResultDtoOfString> DispatchStock(DispatchStockRequest body, CancellationToken cancellationToken = default)
    {
        var result = await _inventoryService.DispatchStock(body.ProductId, (decimal)body.Quantity, body.UnitAbbreviation);
        return new ResultDtoOfString
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }
}
