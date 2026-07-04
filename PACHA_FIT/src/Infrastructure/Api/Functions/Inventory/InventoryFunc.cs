using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Api.Mappers;
using PACHA_FIT.Core.Application.Inventory;
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
    public async Task<ResultDto<UnitOfMeasureGroupedDto>> RunGetUnits(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/units")] HttpRequest req)
    {
        var result = await _unitOfMeasureService.GetAllActiveUnitsAsync();
        return new ResultDto<UnitOfMeasureGroupedDto>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value?.ToApi(),
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    [Function("Inventory_ConvertUnit")]
    public async Task<ResultDto<decimal>> RunConvertUnit(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/inventory/units/convert")] HttpRequest req)
    {
        if (!double.TryParse(req.Query["quantity"], out double quantity)) return new ResultDto<decimal> { IsSuccess = false, Error = "Invalid quantity" };
        string fromUnit = req.Query["fromUnit"]!;
        string toUnit = req.Query["toUnit"]!;

        var result = await _unitOfMeasureService.Convert((decimal)quantity, fromUnit, toUnit);
        return new ResultDto<decimal>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    [Function("Inventory_CreateProduct")]
    public async Task<ResultDto<ProductResponse>> RunCreateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/products")] HttpRequest req)
    {
        var body = await req.ReadFromJsonAsync<ProductCreateRequest>();
        if (body == null) return new ResultDto<ProductResponse> { IsSuccess = false, Error = "Invalid request body" };

        var domainRequest = body.ToDomain();
        var result = await _productsService.CreateProduct(domainRequest);
        return new ResultDto<ProductResponse>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value?.ToApi(),
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    [Function("Inventory_DeactivateProduct")]
    public async Task<ResultDto<bool>> RunDeactivateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/products/{sku}/deactivate")] HttpRequest req, string sku)
    {
        var result = await _productsService.DeactivateProduct(sku);
        return new ResultDto<bool>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }

    [Function("Inventory_DispatchStock")]
    public async Task<ResultDto<string>> RunDispatchStock(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/inventory/dispatch")] HttpRequest req)
    {
        var body = await req.ReadFromJsonAsync<DispatchStockRequest>();
        if (body == null) return new ResultDto<string> { IsSuccess = false, Error = "Invalid request body" };

        var result = await _inventoryService.DispatchStock(body.ProductId, (decimal)body.Quantity, body.UnitAbbreviation);
        return new ResultDto<string>
        {
            IsSuccess = result.IsSuccess,
            Value = result.Value,
            Error = result.Error,
            StatusCode = result.StatusCode
        };
    }
}
