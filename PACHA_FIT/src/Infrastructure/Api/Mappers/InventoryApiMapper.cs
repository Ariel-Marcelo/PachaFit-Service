using PACHA_FIT.Infrastructure.Api.Dtos;
using Riok.Mapperly.Abstractions;

namespace PACHA_FIT.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class InventoryApiMapper
{
    // Product mappings
    [MapProperty(nameof(ProductCreateRequest.Sku), nameof(PACHA_FIT.Core.Domain.Inventory.Dtos.ProductCreateRequest.SKU))]
    public static partial PACHA_FIT.Core.Domain.Inventory.Dtos.ProductCreateRequest ToDomain(this ProductCreateRequest request);
    
    [MapProperty(nameof(PACHA_FIT.Core.Domain.Inventory.Dtos.ProductResponse.SKU), nameof(ProductResponse.Sku))]
    public static partial ProductResponse ToApi(this PACHA_FIT.Core.Domain.Inventory.Dtos.ProductResponse response);
    
    // Unit of Measure mappings
    public static partial UnitOfMeasureGroupedDto ToApi(this PACHA_FIT.Core.Domain.Inventory.Dtos.UnitOfMeasureGroupedDto dto);
}
