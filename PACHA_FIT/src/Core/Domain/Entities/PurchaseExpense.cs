using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Core.Domain.Entities;

public partial class PurchaseExpense
{
    public int PurchaseExpenseId { get; set; }

    public int PurchaseId { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual PurchaseOrder Purchase { get; set; } = null!;
}
