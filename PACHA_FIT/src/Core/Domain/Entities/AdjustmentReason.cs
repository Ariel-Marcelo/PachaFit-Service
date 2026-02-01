using System;
using System.Collections.Generic;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class AdjustmentReason
{
    public int ReasonId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
