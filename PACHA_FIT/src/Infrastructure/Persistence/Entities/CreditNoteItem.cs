using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

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
