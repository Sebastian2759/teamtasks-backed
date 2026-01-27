namespace Domain.QueryModels;
public class DeveloperWorkloadRow
{
    public Guid DeveloperId { get; set; }
    public string DeveloperName { get; set; } = default!;
    public int OpenTasksCount { get; set; }
    public decimal AverageEstimatedComplexity { get; set; }
}
