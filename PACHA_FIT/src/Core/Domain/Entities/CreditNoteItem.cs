using System;
using System.Collections.Generic;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class CreditNoteItem
{
    public int CreditNoteItemId { get; set; }

    public int CreditNoteId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPriceSnapshot { get; set; }

    public decimal TaxAmount { get; set; }

    public virtual CreditNote CreditNote { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
