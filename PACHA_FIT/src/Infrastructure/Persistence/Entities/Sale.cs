using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class Sale
{
    public int SaleId { get; set; }

    public Guid? SaleUuid { get; set; }

    public int? UserId { get; set; }

    public int? PaymentMethodId { get; set; }

    public string? OrderNumber { get; set; }

    public string? Origin { get; set; }

    public string? Status { get; set; }

    public decimal Subtotal { get; set; }

    public decimal TotalTax { get; set; }

    public decimal? ShippingCost { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTimeOffset? SaleDate { get; set; }

    public bool? IsCredit { get; set; }

    public DateTimeOffset? DueDate { get; set; }

    public virtual ICollection<AccountingEntry> AccountingEntries { get; set; } = new List<AccountingEntry>();

    public virtual ICollection<CreditNote> CreditNotes { get; set; } = new List<CreditNote>();

    public virtual ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual User? User { get; set; }
}
