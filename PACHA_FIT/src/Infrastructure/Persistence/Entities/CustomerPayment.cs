using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class CustomerPayment
{
    public int PaymentId { get; set; }

    public int SaleId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset? PaymentDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? Notes { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
