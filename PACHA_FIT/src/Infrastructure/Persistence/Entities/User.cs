namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public string? IdentificationType { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<AccountingPeriod> AccountingPeriodClosedByNavigations { get; set; } = new List<AccountingPeriod>();

    public virtual ICollection<AccountingPeriod> AccountingPeriodLastReopenedByNavigations { get; set; } = new List<AccountingPeriod>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<CreditNote> CreditNotes { get; set; } = new List<CreditNote>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
