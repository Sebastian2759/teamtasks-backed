using Domain.Contracts.Persistence;
using Domain.Entities;
using Domain.QueryModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using Persistence.Context;

namespace Persistence.Repositories;

public class TasksRepository(TeamTasksSampleContext db)
    : RepositoryGeneric<TaskEntity>(db), ITasksRepository
{
    private readonly TeamTasksSampleContext _ctx = db;

    public async Task<PagedResultQueryModel<TaskListItemQueryModel>> GetTasksPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? assignedUserId,
        string? status,
        Guid? priorityId,
        string? tag,
        string? search,
        CancellationToken ct)
    {
        var sql = "EXEC dbo.sp_tasks_get_paged @PageNumber, @PageSize, @AssignedUserId, @Status, @PriorityId, @Tag, @Search";

        SqlParameter[] parameters =
        [
            new SqlParameter("@PageNumber", pageNumber),
            new SqlParameter("@PageSize", pageSize),
            new SqlParameter("@AssignedUserId", (object?)assignedUserId ?? DBNull.Value),
            new SqlParameter("@Status", (object?)status ?? DBNull.Value),
            new SqlParameter("@PriorityId", (object?)priorityId ?? DBNull.Value),
            new SqlParameter("@Tag", (object?)tag ?? DBNull.Value),
            new SqlParameter("@Search", (object?)search ?? DBNull.Value),
        ];

        var rows = await _ctx.TasksList
            .FromSqlRaw(sql, parameters)
            .ToListAsync(ct);

        var total = rows.FirstOrDefault()?.TotalCount ?? 0;

        return new PagedResultQueryModel<TaskListItemQueryModel>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = total,
            Items = rows
        };
    }
}
