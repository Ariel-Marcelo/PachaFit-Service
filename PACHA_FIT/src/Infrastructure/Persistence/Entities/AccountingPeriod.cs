namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class AccountingPeriod
{
    public int PeriodId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public bool? IsClosed { get; set; }

    public DateTimeOffset? ClosedAt { get; set; }

    public int? ClosedBy { get; set; }

    public DateTimeOffset? LastReopenedAt { get; set; }

    public int? LastReopenedBy { get; set; }

    public string? ReopenReason { get; set; }

    public virtual User? ClosedByNavigation { get; set; }

    public virtual User? LastReopenedByNavigation { get; set; }
}
