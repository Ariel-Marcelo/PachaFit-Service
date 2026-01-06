namespace PACHA_FIT.Core.Domain.Entities;

public partial class PurchaseOrder
{
    public int PurchaseId { get; set; }

    public int? SupplierId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? TotalTax { get; set; }

    public int? TaxRateId { get; set; }

    public DateTimeOffset? PurchaseDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<AccountingEntry> AccountingEntries { get; set; } = new List<AccountingEntry>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual Supplier? Supplier { get; set; }

    public virtual TaxRate? TaxRate { get; set; }
}
