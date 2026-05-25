using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Core.Application.Inventory;

public class ProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;

    public ProductsService(IProductRepository productRepository, IStockMovementRepository stockMovementRepository)
    {
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<Result<ProductResponse>> CreateProduct(ProductCreateRequest request)
    {
        // Validation: Unique SKU
        if (await _productRepository.ExistsSku(request.SKU))
        {
            return Result<ProductResponse>.Failure("El SKU ya existe", PACHA_FIT.Core.Domain.Shared.ErrorType.Validation);
        }

        // Validation: Positive prices
        if (request.CostPrice < 0 || request.SalePrice < 0)
        {
            return Result<ProductResponse>.Failure("El precio no puede ser negativo", PACHA_FIT.Core.Domain.Shared.ErrorType.Validation);
        }

        var response = await _productRepository.SaveProduct(request);
        
        // Side effect: Record initial stock movement for the parent product
        await _stockMovementRepository.SaveMovement(new StockMovementRequest(
            ProductId: response.ProductId,
            InputQuantity: request.InitialStock,
            InputUnitAbbreviation: request.InitialUnitAbbreviation, 
            BaseQuantityAffected: request.InitialStock, // Simplification
            TypeMovement: "Ingreso",
            Description: "Carga inicial de producto"
        ));

        // Side effect: If it's a kit with stock, deduct components (Assembly)
        if (request.Composition != null && request.Composition.Any() && request.InitialStock > 0)
        {
            foreach (var component in request.Composition)
            {
                var componentProduct = await _productRepository.GetBySku(component.BaseProductSku);
                
                if (componentProduct == null)
                {
                    return Result<ProductResponse>.Failure($"El producto base {component.BaseProductSku} no existe", PACHA_FIT.Core.Domain.Shared.ErrorType.NotFound);
                }

                var requiredQty = component.Quantity * request.InitialStock;

                if (componentProduct.StockQty < requiredQty)
                {
                    return Result<ProductResponse>.Failure($"Stock insuficiente de {componentProduct.Name} para el ensamble", PACHA_FIT.Core.Domain.Shared.ErrorType.Validation);
                }

                await _stockMovementRepository.SaveMovement(new StockMovementRequest(
                    ProductId: componentProduct.ProductId, 
                    InputQuantity: requiredQty,
                    InputUnitAbbreviation: component.UnitAbbreviation,
                    BaseQuantityAffected: requiredQty, 
                    TypeMovement: "Egreso",
                    Description: $"Descuento por ensamble de Kit: {response.Name}"
                ));
            }
        }

        return Result<ProductResponse>.Success(response);
    }

    public async Task<Result<bool>> DeactivateProduct(string sku)
    {
        return await _productRepository.DeactivateProduct(sku);
    }
}
