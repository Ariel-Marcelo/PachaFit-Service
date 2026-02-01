using System;
using System.Collections.Generic;
using PACHA_FIT.Core.Domain.Entities;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class CreditNote
{
    public int CreditNoteId { get; set; }

    public Guid? CreditNoteUuid { get; set; }

    public int SaleId { get; set; }

    public int? UserId { get; set; }

    public string NoteNumber { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public decimal TotalTax { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public string? AdjustmentType { get; set; }

    public bool? AffectsInventory { get; set; }

    public virtual ICollection<AccountingEntry> AccountingEntries { get; set; } = new List<AccountingEntry>();

    public virtual ICollection<CreditNoteItem> CreditNoteItems { get; set; } = new List<CreditNoteItem>();

    public virtual Sale Sale { get; set; } = null!;

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual User? User { get; set; }
}
