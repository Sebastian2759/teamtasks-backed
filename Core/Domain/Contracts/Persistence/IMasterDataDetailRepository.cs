using Domain.Base.Interface;
using Domain.Entities;


namespace Domain.Contracts.Persistence;

public interface IMasterDataDetailRepository : IRepositoryGeneric<MasterDataDetailEntity>
{
    Task<List<MasterDataDetailEntity>> GetByMasterIdAsync(Guid masterDataId, CancellationToken ct);
}
