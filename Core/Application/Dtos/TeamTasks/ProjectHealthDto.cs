namespace Application.Dtos.TeamTasks;
public class ProjectHealthDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int TotalTasks { get; set; }
    public int OpenTasks { get; set; }
    public int CompletedTasks { get; set; }
}
