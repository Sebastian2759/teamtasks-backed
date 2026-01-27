using Domain.QueryModels;
namespace Domain.Contracts.Persistence;

public interface ITasksRepository
{
    Task<(IReadOnlyList<ProjectTaskPagedRow> Items, int TotalCount)> GetProjectTasksPagedAsync(
       Guid projectId,
       Guid? idTaskStatus,
       Guid? assigneeId,
       int page,
       int pageSize,
       CancellationToken ct);

    Task<CreatedTaskRow> CreateTaskAsync(CreateTaskSpParams input, CancellationToken ct);
}