using PACHA_FIT.src.Core.Domain.Entities;

namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public int? ParentAccountId { get; set; }

    public bool? IsPostable { get; set; }

    public virtual ICollection<AccountingEntry> AccountingEntries { get; set; } = new List<AccountingEntry>();

    public virtual ICollection<Account> InverseParentAccount { get; set; } = new List<Account>();

    public virtual Account? ParentAccount { get; set; }

    public virtual ICollection<PurchaseExpense> PurchaseExpenses { get; set; } = new List<PurchaseExpense>();
}
