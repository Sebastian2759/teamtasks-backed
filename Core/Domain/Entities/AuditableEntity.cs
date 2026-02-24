namespace Domain.Entities;

public abstract class AuditableEntity : Base.EntityBase
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
