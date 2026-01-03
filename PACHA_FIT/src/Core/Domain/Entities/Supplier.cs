using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Core.Domain.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? TaxId { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactEmail { get; set; }

    public string? Attributes { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
