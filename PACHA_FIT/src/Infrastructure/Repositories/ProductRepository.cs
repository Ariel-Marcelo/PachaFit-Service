using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;
using PACHA_FIT.Infrastructure.Persistence;
using PACHA_FIT.Infrastructure.Persistence.Entities;
using System.Text.Json;

namespace PACHA_FIT.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PachaFitContext _context;

    public ProductRepository(PachaFitContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsSku(string sku)
    {
        return await _context.Products.AnyAsync(p => p.Sku == sku);
    }

    public async Task<ProductResponse> GetBySku(string sku)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.ProductCompositionParentProducts)
                .ThenInclude(c => c.BaseProduct)
            .FirstOrDefaultAsync(p => p.Sku == sku);

        if (product == null) return null!;

        return MapToResponse(product);
    }

    public async Task<ProductResponse> SaveProduct(ProductCreateRequest request)
    {
        var slug = request.Name.ToLower().Replace(" ", "-"); // Basic slugification
        
        var product = new Product
        {
            Name = request.Name,
            Sku = request.SKU,
            Slug = slug,
            CategoryId = request.CategoryId,
            CostPrice = request.CostPrice,
            SalePrice = request.SalePrice,
            IvaPercentage = request.IvaPercentage,
            IsWeightBased = request.IsWeightBased,
            StockQty = request.InitialStock,
            Specs = request.Specs != null ? JsonSerializer.Serialize(request.Specs) : null,
            IsPublished = true
        };

        // Get Unit ID if abbreviation provided
        if (!string.IsNullOrEmpty(request.InitialUnitAbbreviation))
        {
            var unit = await _context.UnitOfMeasures.FirstOrDefaultAsync(u => u.Abbreviation == request.InitialUnitAbbreviation);
            if (unit != null)
            {
                product.StockUnitId = unit.UnitId;
                product.SaleUnitId = unit.UnitId;
                product.PurchaseUnitId = unit.UnitId;
            }
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Handle Composition if any
        if (request.Composition != null)
        {
            foreach (var comp in request.Composition)
            {
                var baseProduct = await _context.Products.FirstOrDefaultAsync(p => p.Sku == comp.BaseProductSku);
                var unit = await _context.UnitOfMeasures.FirstOrDefaultAsync(u => u.Abbreviation == comp.UnitAbbreviation);
                
                if (baseProduct != null && unit != null)
                {
                    _context.ProductCompositions.Add(new ProductComposition
                    {
                        ParentProductId = product.ProductId,
                        BaseProductId = baseProduct.ProductId,
                        QuantityUsed = comp.Quantity,
                        UnitId = unit.UnitId
                    });
                }
            }
            await _context.SaveChangesAsync();
        }

        return MapToResponse(product);
    }

    public async Task<Result<bool>> UpdateProduct(string sku, object updateData)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Sku == sku);
        if (product == null) return Result<bool>.Failure("Producto no encontrado", PACHA_FIT.Core.Domain.Shared.ErrorType.NotFound);

        // Simple update logic for now (could be more robust with reflection or specific DTO)
        // Since it's dynamic updateData, I'll just handle basic fields if needed or skip for now if not used.
        // The feature mentions updating Name and SalePrice.
        
        var json = JsonSerializer.Serialize(updateData);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json, options);

        if (data != null)
        {
            if (data.TryGetValue("Name", out var name)) product.Name = name.ToString()!;
            if (data.TryGetValue("SalePrice", out var salePrice)) product.SalePrice = decimal.Parse(salePrice.ToString()!);
            // ... add more as needed
        }

        await _context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeactivateProduct(string sku)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Sku == sku);
        if (product == null) return Result<bool>.Failure("Producto no encontrado", PACHA_FIT.Core.Domain.Shared.ErrorType.NotFound);

        product.IsPublished = false;
        await _context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    private ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            ProductId: product.ProductId,
            Name: product.Name,
            SKU: product.Sku,
            StockQty: product.StockQty ?? 0,
            IvaPercentage: product.IvaPercentage,
            IsWeightBased: product.IsWeightBased,
            SpecsJson: product.Specs,
            Composition: product.ProductCompositionParentProducts?.Select(c => new ProductCompositionResponse(
                BaseProductName: c.BaseProduct.Name,
                Quantity: c.QuantityUsed,
                UnitAbbreviation: c.Unit?.Abbreviation ?? ""
            )).ToList()
        );
    }
}
