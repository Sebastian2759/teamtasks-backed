using Domain.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using Persistence.Context;

namespace Persistence.Repositories;

public class ReferencialDataRepository(TeamTasksSampleContext context) : RepositoryGeneric<ReferencialDataEntity>(context), IReferencialDataRepository
{
    private readonly TeamTasksSampleContext _contexto = context;

    public async Task<IEnumerable<ReferencialDataDetailsEntity>> GetReferencialDataById(Guid id)
    {
        var referencialDatas = await (from rd in _contexto.ReferencialData
                                      join rdd in _contexto.ReferencialDataDetails
                                      on rd.Id equals rdd.IdReferencialData
                                      where rd.Id == id
                                      select new ReferencialDataDetailsEntity
                                      {
                                          Id = rdd.Id,
                                          Description = rdd.Description,
                                          IdReferencialData = rdd.IdReferencialData
                                      }).ToListAsync();

        return referencialDatas;
    }
}