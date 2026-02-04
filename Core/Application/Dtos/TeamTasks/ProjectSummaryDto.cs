namespace Application.Dtos.TeamTasks;

public class ProjectSummaryDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string ProjectName { get; set; }
    public int TotalTasks { get; set; }
    public int OpenTasks { get; set; }
    public int CompletedTasks { get; set; }
}