namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? TaxId { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactEmail { get; set; }

    public string? Attributes { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
