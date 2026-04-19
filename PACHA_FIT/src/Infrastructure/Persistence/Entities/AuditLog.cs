namespace PACHA_FIT.Infrastructure.Persistence.Entities;

public partial class AuditLog
{
    public long AuditId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string RecordId { get; set; } = null!;

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
