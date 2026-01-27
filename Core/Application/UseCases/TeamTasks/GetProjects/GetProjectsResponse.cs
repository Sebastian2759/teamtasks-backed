using Application.Dtos.TeamTasks;

namespace Application.UseCases.TeamTasks.GetProjects;

public class GetProjectsResponse
{
    public IEnumerable<ProjectSummaryDto> Projects { get; set; } = new List<ProjectSummaryDto>();
}