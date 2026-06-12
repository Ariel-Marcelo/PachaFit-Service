using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Core.Domain.Inventory.Ports;

public interface IProductRepository
{
    Task<ProductResponse> SaveProduct(ProductCreateRequest request);
    Task<bool> ExistsSku(string sku);
    Task<ProductResponse> GetBySku(string sku);
    Task<Result<bool>> UpdateProduct(string sku, object updateData);
    Task<Result<bool>> DeactivateProduct(string sku);
}
