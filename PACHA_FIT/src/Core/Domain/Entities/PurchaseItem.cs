using System;
using System.Collections.Generic;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class PurchaseItem
{
    public int PurchaseItemId { get; set; }

    public int? PurchaseId { get; set; }

    public int? ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitCostSnapshot { get; set; }

    public decimal TaxRateSnapshot { get; set; }

    public decimal Subtotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal LineTotal { get; set; }

    public virtual Product? Product { get; set; }

    public virtual PurchaseOrder? Purchase { get; set; }
}
