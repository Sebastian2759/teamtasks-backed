using Domain.Base;

namespace Domain.Entities;

public sealed class TaskEntity : EntityBase
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid AssignedUserId { get; set; }
    public Guid StatusId { get; set; }
    public Guid? PriorityId { get; set; }
    public string? AdditionalInfo { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}