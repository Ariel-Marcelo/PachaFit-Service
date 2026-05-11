using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public Guid? ProductUuid { get; set; }

    public int? CategoryId { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public bool? ManageStockDirectly { get; set; }

    public int? PurchaseUnitId { get; set; }

    public int? SaleUnitId { get; set; }

    public int? StockUnitId { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SalePrice { get; set; }

    public int IvaPercentage { get; set; }

    public bool IsWeightBased { get; set; }

    public decimal? StockQty { get; set; }

    public decimal? MinStockLevel { get; set; }

    public string? MainImageUrl { get; set; }

    public bool? IsPublished { get; set; }

    public string? Specs { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<CreditNoteItem> CreditNoteItems { get; set; } = new List<CreditNoteItem>();

    public virtual ICollection<ProductComposition> ProductCompositionBaseProducts { get; set; } = new List<ProductComposition>();

    public virtual ICollection<ProductComposition> ProductCompositionParentProducts { get; set; } = new List<ProductComposition>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual UnitOfMeasure? PurchaseUnit { get; set; }

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual UnitOfMeasure? SaleUnit { get; set; }

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual UnitOfMeasure? StockUnit { get; set; }
}
