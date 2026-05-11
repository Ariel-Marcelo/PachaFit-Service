using PACHA_FIT.Core.Domain.Inventory.Dtos;

namespace PACHA_FIT.Core.Domain.Inventory.Ports;

public interface IProductRepository
{
    Task<ProductResponse> SaveProduct(ProductCreateRequest request);
}
