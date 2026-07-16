using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class SaleItem
{
    public int SaleItemId { get; set; }

    public int? SaleId { get; set; }

    public int? ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPriceSnapshot { get; set; }

    public decimal CostPriceSnapshot { get; set; }

    public int TaxRateId { get; set; }

    public decimal TaxRateSnapshot { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal LineTotal { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Sale? Sale { get; set; }

    public virtual TaxRate? TaxRate { get; set; }
}
