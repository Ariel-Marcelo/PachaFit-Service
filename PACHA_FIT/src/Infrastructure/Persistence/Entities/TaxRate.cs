using System;
using System.Collections.Generic;
using PACHA_FIT.Infrastructure.Persistence.Entities;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class TaxRate
{
    public int TaxRateId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Rate { get; set; }

    public bool? IsActive { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
