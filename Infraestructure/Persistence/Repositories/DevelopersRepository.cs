using Domain.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class DevelopersRepository(TeamTasksSampleContext context) : IDevelopersRepository
{
    private readonly TeamTasksSampleContext _contexto = context;

    public async Task<IEnumerable<DeveloperEntity>> GetActiveDevelopers()
    {
        var devs = await _contexto.Developers
           .AsNoTracking()
           .Where(x => x.IsActive)
           .Select(x => new DeveloperEntity
           {
               DeveloperId = x.DeveloperId,
               FirstName = x.FirstName,
               LastName = x.LastName,
               Email = x.Email,
               IsActive = x.IsActive,
               CreatedAts = x.CreatedAts
           })
           .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
           .ToListAsync();

        return devs;
    }
}