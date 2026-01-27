using Domain.QueryModels;

namespace Domain.Contracts.Persistence;

public interface IDashboardRepository
{
    Task<List<DeveloperWorkloadRow>> GetDeveloperWorkloadAsync(CancellationToken ct);
    Task<List<ProjectHealthRow>> GetProjectHealthAsync(CancellationToken ct);
    Task<List<DeveloperDelayRiskRow>> GetDeveloperDelayRiskAsync(CancellationToken ct);
}
