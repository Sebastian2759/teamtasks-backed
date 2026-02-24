using Domain.Base.Interface;
using Domain.Entities;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Persistence
{
    public interface ITasksRepository : IRepositoryGeneric<TaskEntity>
    {
        Task<PagedResultQueryModel<TaskListItemQueryModel>> GetTasksPagedAsync(
            int pageNumber,
            int pageSize,
            Guid? assignedUserId,
            string? status,
            Guid? priorityId,
            string? tag,
            string? search,
            CancellationToken ct);
    }
}
