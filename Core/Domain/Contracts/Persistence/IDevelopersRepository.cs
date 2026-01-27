using Domain.Entities;

namespace Domain.Contracts.Persistence;

public interface IDevelopersRepository
{
    Task<IEnumerable<DeveloperEntity>> GetActiveDevelopers();
}