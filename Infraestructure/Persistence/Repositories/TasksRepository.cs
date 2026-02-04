using Domain.Contracts.Persistence;
using Domain.Entities;
using Domain.QueryModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using Persistence.Context;

namespace Persistence.Repositories;

public class TasksRepository(TeamTasksSampleContext db) : RepositoryGeneric<ReferencialDataEntity>(db), ITasksRepository
{
    private readonly TeamTasksSampleContext _db = db;

    public async Task<CreatedTaskRow> CreateTaskAsync(CreateTaskSpParams input, CancellationToken ct)
    {
        try
        {
            string sql = @"EXEC dbo.TT_SP_CreateTask
                    @ProjectId,
                    @Title,
                    @Description,
                    @AssigneeId,
                    @IdTaskStatus,
                    @IdTaskPriority,
                    @EstimatedComplexity,
                    @DueDate,
                    @CompletionDate";

            SqlParameter p1 = new("@ProjectId", input.ProjectId);
            SqlParameter p2 = new("@Title", input.Title);
            SqlParameter p3 = new("@Description", (object?)input.Description ?? DBNull.Value);
            SqlParameter p4 = new("@AssigneeId", input.AssigneeId);
            SqlParameter p5 = new("@IdTaskStatus", input.IdTaskStatus);
            SqlParameter p6 = new("@IdTaskPriority", input.IdTaskPriority);
            SqlParameter p7 = new("@EstimatedComplexity", input.EstimatedComplexity);
            SqlParameter p8 = new("@DueDate", input.DueDate.Date);
            SqlParameter p9 = new("@CompletionDate", (object?)input.CompletionDate ?? DBNull.Value);

            var rows = await _db.CreatedTaskRows
                .FromSqlRaw(sql, p1, p2, p3, p4, p5, p6, p7, p8, p9)
                .AsNoTracking()
                .ToListAsync(ct);

            var created = rows.FirstOrDefault();

            if (created is null)
                throw new InvalidOperationException("No fue posible crear la tarea: el SP no devolvió resultado.");

            return created!;
        }
        catch (Exception)
        {

            throw new InvalidOperationException("No fue posible crear la tarea: el SP no devolvió resultado.");
        }
       
    }

    public async Task<(IReadOnlyList<ProjectTaskPagedRow> Items, int TotalCount)> GetProjectTasksPagedAsync(Guid projectId, Guid? idTaskStatus, Guid? assigneeId, int page, int pageSize, CancellationToken ct)
    {
        var pProjectId = new SqlParameter("@ProjectId", projectId);

        var pStatus = new SqlParameter("@IdTaskStatus", System.Data.SqlDbType.UniqueIdentifier);
        pStatus.Value = idTaskStatus.HasValue ? idTaskStatus.Value : DBNull.Value;

        var pAssignee = new SqlParameter("@AssigneeId", System.Data.SqlDbType.UniqueIdentifier);
        pAssignee.Value = assigneeId.HasValue ? assigneeId.Value : DBNull.Value;

        var pPage = new SqlParameter("@Page", page);
        var pPageSize = new SqlParameter("@PageSize", pageSize);

        var sql = "EXEC dbo.TT_SP_GetProjectTasksPaged @ProjectId, @IdTaskStatus, @AssigneeId, @Page, @PageSize";

        var rows = await _db.ProjectTaskPagedRows
            .FromSqlRaw(sql, pProjectId, pStatus, pAssignee, pPage, pPageSize)
            .AsNoTracking()
            .ToListAsync(ct);

        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        return (rows, total);
    }

    public async Task<IEnumerable<ProjectTaskStatusSummaryRow>> GetProjectTaskStatusSummaryAsync(
       Guid projectId,
       CancellationToken ct)
    {
        const string sql = @"EXEC dbo.TT_SP_ProjectTaskStatusSummary @ProjectId";

        var p1 = new SqlParameter("@ProjectId", projectId);

        return await _db.ProjectTaskStatusSummaryRows
            .FromSqlRaw(sql, p1)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
