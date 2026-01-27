using Domain.Entities;

namespace Domain.Contracts.Persistence;

public interface IProjectsRepository
{
    Task<List<ProjectEntity>> GetAllAsync(CancellationToken ct);
    Task<ProjectEntity> GetByIdAsync(Guid projectId, CancellationToken ct);
}
