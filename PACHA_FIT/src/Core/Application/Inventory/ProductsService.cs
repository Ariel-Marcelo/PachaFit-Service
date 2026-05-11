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
                await _stockMovementRepository.SaveMovement(new StockMovementRequest(
                    ProductId: 0, 
                    InputQuantity: component.Quantity * request.InitialStock,
                    InputUnitAbbreviation: component.UnitAbbreviation,
                    BaseQuantityAffected: component.Quantity * request.InitialStock, 
                    TypeMovement: "Egreso",
                    Description: $"Descuento por ensamble de Kit: {response.Name}"
                ));
            }
        }

        return Result<ProductResponse>.Success(response);
    }
}