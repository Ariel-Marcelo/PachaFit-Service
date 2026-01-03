namespace PACHA_FIT.Core.Domain.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public virtual ICollection<AccountingEntry> AccountingEntries { get; set; } = new List<AccountingEntry>();
}
