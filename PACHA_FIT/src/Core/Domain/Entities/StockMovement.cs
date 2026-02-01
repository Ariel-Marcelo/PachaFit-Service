using System;
using System.Collections.Generic;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class StockMovement
{
    public long MovementId { get; set; }

    public int? ProductId { get; set; }

    public decimal InputQuantity { get; set; }

    public int? InputUnitId { get; set; }

    public decimal BaseQuantityAffected { get; set; }

    public string? TypeMovement { get; set; }

    public int? SaleId { get; set; }

    public int? PurchaseId { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public int? AdjustmentReasonId { get; set; }

    public int? CreditNoteId { get; set; }

    public virtual AdjustmentReason? AdjustmentReason { get; set; }

    public virtual CreditNote? CreditNote { get; set; }

    public virtual UnitOfMeasure? InputUnit { get; set; }

    public virtual Product? Product { get; set; }

    public virtual PurchaseOrder? Purchase { get; set; }

    public virtual Sale? Sale { get; set; }
}
