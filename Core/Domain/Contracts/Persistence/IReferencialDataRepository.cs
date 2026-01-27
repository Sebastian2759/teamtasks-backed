using Domain.Entities;

namespace Domain.Contracts.Persistence;

public interface IReferencialDataRepository
{
    Task<IEnumerable<ReferencialDataDetailsEntity>> GetReferencialDataById(Guid id);
}