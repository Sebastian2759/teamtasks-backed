using Domain.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using Persistence.Context;

namespace Persistence.Repositories;

public class MasterDataDetailRepository : RepositoryGeneric<MasterDataDetailEntity>, IMasterDataDetailRepository
{
    private readonly TeamTasksSampleContext _ctx;

    public MasterDataDetailRepository(TeamTasksSampleContext db) : base(db)
    {
        _ctx = db;
    }

    public Task<List<MasterDataDetailEntity>> GetByMasterIdAsync(Guid masterDataId, CancellationToken ct)
       => _ctx.MasterDataDetail 
           .AsNoTracking()
           .Where(x => x.MasterDataId == masterDataId && x.IsActive)
           .OrderBy(x => x.Name)
           .ToListAsync(ct);
}
