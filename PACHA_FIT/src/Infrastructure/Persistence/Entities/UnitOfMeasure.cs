namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class UnitOfMeasure
{
    public int UnitId { get; set; }

    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal ConversionFactor { get; set; }

    public virtual ICollection<ProductComposition> ProductCompositions { get; set; } = new List<ProductComposition>();

    public virtual ICollection<Product> ProductPurchaseUnits { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductSaleUnits { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductStockUnits { get; set; } = new List<Product>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
