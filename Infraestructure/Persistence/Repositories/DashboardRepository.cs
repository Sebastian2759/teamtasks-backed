using Domain.Contracts.Persistence;
using Domain.Entities;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using Persistence.Context;

namespace Persistence.Repositories;

public class DashboardRepository(TeamTasksSampleContext db) : RepositoryGeneric<ReferencialDataEntity>(db), IDashboardRepository
{
    private readonly TeamTasksSampleContext _db = db;
    private Guid? _completedStatusIdCache;
    private async Task<Guid> GetCompletedStatusIdAsync(CancellationToken ct)
    {
        if (_completedStatusIdCache.HasValue) return _completedStatusIdCache.Value;

        var completedId = await (
            from m in _db.TB_ReferencialData.AsNoTracking()
            join d in _db.TB_ReferencialData_Details.AsNoTracking()
                on m.Id equals d.IdReferencialData
            where m.Description == "ESTADOS DE TAREA"
               && d.Description == "Completada"
               && m.Valid == true
               && d.Valid == true
            select d.Id
        ).FirstOrDefaultAsync(ct);

        if (completedId == Guid.Empty)
            throw new InvalidOperationException("No se encontró el estado 'Completada' en referenciales.");

        _completedStatusIdCache = completedId;
        return completedId;
    }

    public async Task<List<DeveloperWorkloadRow>> GetDeveloperWorkloadAsync(CancellationToken ct)
    {
        var completedId = await GetCompletedStatusIdAsync(ct);

        var query =
            from dev in _db.Developers.AsNoTracking()
            where dev.IsActive
            join t in _db.Tasks.AsNoTracking().Where(x => x.IdTaskStatus != completedId)
                on dev.DeveloperId equals t.AssigneeId into tg
            select new DeveloperWorkloadRow
            {
                DeveloperId = dev.DeveloperId,
                DeveloperName = dev.FirstName + " " + dev.LastName,
                OpenTasksCount = tg.Count(),
                AverageEstimatedComplexity = (decimal)(tg.Select(x => (decimal?)x.EstimatedComplexity).Average() ?? 0)
            };

        return await query
            .OrderBy(x => x.DeveloperName)
            .ToListAsync(ct);
    }

    public async Task<List<ProjectHealthRow>> GetProjectHealthAsync(CancellationToken ct)
    {
        var completedId = await GetCompletedStatusIdAsync(ct);

        var query =
            from p in _db.Projects.AsNoTracking()
            join st in _db.TB_ReferencialData_Details.AsNoTracking()
                on p.IdProjectStatus equals st.Id
            join t in _db.Tasks.AsNoTracking()
                on p.ProjectId equals t.ProjectId into tg
            select new ProjectHealthRow
            {
                ProjectId = p.ProjectId,
                ProjectName = p.Name,
                ClientName = p.ClientName,
                TotalTasks = tg.Count(),
                CompletedTasks = tg.Count(x => x.IdTaskStatus == completedId),
                OpenTasks = tg.Count(x => x.IdTaskStatus != completedId),
                Status = st.Description
            };

        return await query
            .OrderBy(x => x.ProjectName)
            .ToListAsync(ct);
    }

    public async Task<List<DeveloperDelayRiskRow>> GetDeveloperDelayRiskAsync(CancellationToken ct)
    {
        return await _db.DeveloperDelayRiskRows
            .AsNoTracking()
            .OrderByDescending(x => x.HighRiskFlag)
            .ThenByDescending(x => x.AvgDelayDays)
            .ThenByDescending(x => x.OpenTasksCount)
            .ToListAsync(ct);
    }
}