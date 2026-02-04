namespace Domain.QueryModels;

public class ProjectTaskStatusSummaryRow
{
    public Guid IdTaskStatus { get; set; }
    public string Status { get; set; } = default!;
    public int TasksCount { get; set; }
}
