namespace Application.Dtos.TeamTasks;

public class TaskDto
{
    public Guid TaskId { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public Guid AssigneeId { get; set; }
    public string AssigneeName { get; set; } = default!;

    public Guid IdTaskStatus { get; set; }
    public string Status { get; set; } = default!;

    public Guid IdTaskPriority { get; set; }
    public string Priority { get; set; } = default!;

    public int EstimatedComplexity { get; set; }
    public DateTime CreatedAts { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
}