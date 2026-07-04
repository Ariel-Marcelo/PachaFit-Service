using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PACHA_FIT.Infrastructure.Api.Dtos;

public class ProductCreateRequest
{
    [JsonPropertyName("name")]
    [Required(AllowEmptyStrings = true)]
    public string Name { get; set; } = null!;

    [JsonPropertyName("sku")]
    [Required(AllowEmptyStrings = true)]
    public string Sku { get; set; } = null!;

    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }

    [JsonPropertyName("costPrice")]
    public double CostPrice { get; set; }

    [JsonPropertyName("salePrice")]
    public double SalePrice { get; set; }

    [JsonPropertyName("ivaPercentage")]
    public int IvaPercentage { get; set; }

    [JsonPropertyName("isWeightBased")]
    public bool IsWeightBased { get; set; }

    [JsonPropertyName("initialStock")]
    public double InitialStock { get; set; }

    [JsonPropertyName("initialUnitAbbreviation")]
    public string InitialUnitAbbreviation { get; set; } = null!;

    [JsonPropertyName("specs")]
    public ICollection<ProductSpec>? Specs { get; set; }

    [JsonPropertyName("composition")]
    public ICollection<ProductCompositionRequest>? Composition { get; set; }
}

public class ProductResponse
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("sku")]
    public string Sku { get; set; } = null!;

    [JsonPropertyName("stockQty")]
    public double StockQty { get; set; }

    [JsonPropertyName("ivaPercentage")]
    public int IvaPercentage { get; set; }

    [JsonPropertyName("isWeightBased")]
    public bool IsWeightBased { get; set; }

    [JsonPropertyName("specsJson")]
    public string? SpecsJson { get; set; }

    [JsonPropertyName("composition")]
    public ICollection<ProductCompositionResponse>? Composition { get; set; }
}

public class ProductSpec
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = null!;

    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;
}

public class ProductCompositionRequest
{
    [JsonPropertyName("baseProductSku")]
    public string BaseProductSku { get; set; } = null!;

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("unitAbbreviation")]
    public string UnitAbbreviation { get; set; } = null!;
}

public class ProductCompositionResponse
{
    [JsonPropertyName("baseProductName")]
    public string BaseProductName { get; set; } = null!;

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("unitAbbreviation")]
    public string UnitAbbreviation { get; set; } = null!;
}

public class UnitOfMeasureGroupedDto
{
    [JsonPropertyName("massUnits")]
    public ICollection<UnitOfMeasureItemDto> MassUnits { get; set; } = new List<UnitOfMeasureItemDto>();

    [JsonPropertyName("volumeUnits")]
    public ICollection<UnitOfMeasureItemDto> VolumeUnits { get; set; } = new List<UnitOfMeasureItemDto>();

    [JsonPropertyName("discreteUnits")]
    public ICollection<UnitOfMeasureItemDto> DiscreteUnits { get; set; } = new List<UnitOfMeasureItemDto>();
}

public class UnitOfMeasureItemDto
{
    [JsonPropertyName("abbreviation")]
    public string Abbreviation { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}

public class DispatchStockRequest
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("unitAbbreviation")]
    [Required(AllowEmptyStrings = true)]
    public string UnitAbbreviation { get; set; } = null!;
}
