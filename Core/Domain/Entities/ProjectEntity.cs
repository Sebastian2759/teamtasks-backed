namespace Domain.Entities;

public class ProjectEntity
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid IdProjectStatus { get; set; }
    public DateTime CreatedAts { get; set; }
}
