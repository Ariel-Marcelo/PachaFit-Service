namespace PACHA_FIT.Core.Domain.Entities;

public partial class AccountingEntry
{
    public long EntryId { get; set; }

    public int? AccountId { get; set; }

    public int? SaleId { get; set; }

    public int? PurchaseId { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? EntryDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual PurchaseOrder? Purchase { get; set; }

    public virtual Sale? Sale { get; set; }
}
