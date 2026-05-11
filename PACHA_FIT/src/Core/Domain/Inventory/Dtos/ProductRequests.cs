namespace PACHA_FIT.Core.Domain.Inventory.Dtos;

public record ProductCreateRequest(
    string Name,
    string SKU,
    int? CategoryId = null,
    decimal CostPrice = 0, // Reference average cost price
    decimal SalePrice = 0, // Current reference sale price
    int IvaPercentage = 0,
    bool IsWeightBased = false,
    decimal InitialStock = 0,
    string InitialUnitAbbreviation = "u",
    List<ProductSpec>? Specs = null,
    List<ProductCompositionRequest>? Composition = null
);

public record ProductResponse(
    int ProductId,
    string Name,
    string SKU,
    decimal StockQty,
    int IvaPercentage,
    bool IsWeightBased,
    string? SpecsJson,
    List<ProductCompositionResponse>? Composition = null
);

public record ProductSpec(
    string Label,
    string Value
);

public record ProductCompositionRequest(
    string BaseProductSku,
    decimal Quantity,
    string UnitAbbreviation
);

public record ProductCompositionResponse(
    string BaseProductName,
    decimal Quantity,
    string UnitAbbreviation
);
