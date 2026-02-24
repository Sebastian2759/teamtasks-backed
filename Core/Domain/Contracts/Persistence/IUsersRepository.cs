using Domain.Base.Interface;
using Domain.Entities;
using Domain.QueryModels;

namespace Domain.Contracts.Persistence;

public interface IUsersRepository : IRepositoryGeneric<UserEntity>
{
    Task<PagedResultQueryModel<UserListItemQueryModel>> GetUsersPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        bool? isActive,
        CancellationToken ct);
}
