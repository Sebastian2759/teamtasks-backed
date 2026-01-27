namespace Domain.Entities;

public class TaskEntity
{
    public Guid TaskId { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public Guid AssigneeId { get; set; }

    public Guid IdTaskStatus { get; set; }
    public Guid IdTaskPriority { get; set; }

    public int EstimatedComplexity { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }

    public DateTime CreatedAts { get; set; }
}
