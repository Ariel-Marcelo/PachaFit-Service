using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api.Mappers;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Infrastructure.Api.Dtos;

namespace PACHA_FIT.Api.Functions.Inventory;

public class InventoryFunc
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
        return await Task.FromResult<ICollection<string>>(_adjustmentReasons.GetAllReasons().ToList());
    }

    [Function("Inventory_GetUnits")]
    public async Task<Result<UnitOfMeasureGroupedDto>> RunGetUnits(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/units")] HttpRequest req)
    {
        var result = await _unitOfMeasureService.GetAllActiveUnitsAsync();
        return result.IsSuccess && result.Value != null
            ? Result<UnitOfMeasureGroupedDto>.Success(result.Value.ToApi())
            : Result<UnitOfMeasureGroupedDto>.Failure(result.Error!, result.StatusCode);
    }

    [Function("Inventory_ConvertUnit")]
    public async Task<Result<decimal>> RunConvertUnit(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/units/convert")] HttpRequest req)
    {
        if (!double.TryParse(req.Query["quantity"], out double quantity)) 
            return Result<decimal>.Failure("Invalid quantity", ErrorType.BadRequest);
            
        string fromUnit = req.Query["fromUnit"]!;
        string toUnit = req.Query["toUnit"]!;

        return await _unitOfMeasureService.Convert((decimal)quantity, fromUnit, toUnit);
    }

    [Function("Inventory_CreateProduct")]
    public async Task<Result<ProductResponse>> RunCreateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/products")] HttpRequest req)
    {
        var body = await req.ReadFromJsonAsync<ProductCreateRequest>();
        if (body == null) 
            return Result<ProductResponse>.Failure("Invalid request body", ErrorType.BadRequest);

        var domainRequest = body.ToDomain();
        var result = await _productsService.CreateProduct(domainRequest);
        return result.IsSuccess && result.Value != null
            ? Result<ProductResponse>.Success(result.Value.ToApi())
            : Result<ProductResponse>.Failure(result.Error!, result.StatusCode);
    }

    [Function("Inventory_DeactivateProduct")]
    public async Task<Result<bool>> RunDeactivateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/products/{sku}/deactivate")] HttpRequest req, string sku)
    {
        return await _productsService.DeactivateProduct(sku);
    }

    [Function("Inventory_DispatchStock")]
    public async Task<Result<string>> RunDispatchStock(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/dispatch")] HttpRequest req)
    {
        var body = await req.ReadFromJsonAsync<DispatchStockRequest>();
        if (body == null) 
            return Result<string>.Failure("Invalid request body", ErrorType.BadRequest);

        return await _inventoryService.DispatchStock(body.ProductId, (decimal)body.Quantity, body.UnitAbbreviation);
    }
}
