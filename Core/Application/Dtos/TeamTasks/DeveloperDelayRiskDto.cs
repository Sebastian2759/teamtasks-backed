namespace Application.Dtos.TeamTasks;

public class DeveloperDelayRiskDto
{
    public Guid DeveloperId { get; set; }
    public string DeveloperName { get; set; } = default!;
    public int OpenTasksCount { get; set; }
    public decimal AvgDelayDays { get; set; }
    public DateTime? NearestDueDate { get; set; }
    public DateTime? LatestDueDate { get; set; }
    public DateTime? PredictedCompletionDate { get; set; }
    public int HighRiskFlag { get; set; }
}
