using Application.Dtos.TeamTasks;

namespace Application.UseCases.TeamTasks.Dashboard.GetProjectHealth;

public class GetProjectHealthResponse
{
    public IEnumerable<ProjectHealthDto> Projects { get; set; } = new List<ProjectHealthDto>();
}
