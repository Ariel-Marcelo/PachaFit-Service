using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Infrastructure.Nswag;
using Riok.Mapperly.Abstractions;

namespace PACHA_FIT.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class InventoryApiMapper
{
    // Product mappings
    [MapProperty(nameof(PACHA_FIT.Infrastructure.Nswag.ProductCreateRequest.Sku), nameof(PACHA_FIT.Core.Domain.Inventory.Dtos.ProductCreateRequest.SKU))]
    public static partial PACHA_FIT.Core.Domain.Inventory.Dtos.ProductCreateRequest ToDomain(this PACHA_FIT.Infrastructure.Nswag.ProductCreateRequest request);
    
    [MapProperty(nameof(PACHA_FIT.Core.Domain.Inventory.Dtos.ProductResponse.SKU), nameof(PACHA_FIT.Infrastructure.Nswag.ProductResponse.Sku))]
    public static partial PACHA_FIT.Infrastructure.Nswag.ProductResponse ToApi(this PACHA_FIT.Core.Domain.Inventory.Dtos.ProductResponse response);
    
    // Unit of Measure mappings
    public static partial PACHA_FIT.Infrastructure.Nswag.UnitOfMeasureGroupedDto ToApi(this PACHA_FIT.Core.Domain.Inventory.Dtos.UnitOfMeasureGroupedDto dto);
}
