using Application.Dtos.TeamTasks;

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperWorkload;

public class GetDeveloperWorkloadResponse
{
    public IEnumerable<DeveloperWorkloadDto> Developers { get; set; } = new List<DeveloperWorkloadDto>();
}