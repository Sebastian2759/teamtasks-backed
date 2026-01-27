using Application.Dtos.TeamTasks;

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperDelayRisk;

public class GetDeveloperDelayRiskResponse
{
    public IEnumerable<DeveloperDelayRiskDto> Developers { get; set; } = new List<DeveloperDelayRiskDto>();
}
