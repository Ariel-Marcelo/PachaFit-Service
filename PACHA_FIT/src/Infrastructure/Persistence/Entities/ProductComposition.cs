using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class ProductComposition
{
    public int CompositionId { get; set; }

    public int? ParentProductId { get; set; }

    public int? BaseProductId { get; set; }

    public decimal QuantityUsed { get; set; }

    public int? UnitId { get; set; }

    public virtual Product? BaseProduct { get; set; }

    public virtual Product? ParentProduct { get; set; }

    public virtual UnitOfMeasure? Unit { get; set; }
}
