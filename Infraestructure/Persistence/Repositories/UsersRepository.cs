using Domain.Contracts.Persistence;
using Domain.Entities;
using Domain.QueryModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence.Base;
using Persistence.Context;

namespace Persistence.Repositories;

public class UsersRepository(TeamTasksSampleContext db)
    : RepositoryGeneric<UserEntity>(db), IUsersRepository
{
    private readonly TeamTasksSampleContext _ctx = db;

    public async Task<PagedResultQueryModel<UserListItemQueryModel>> GetUsersPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        bool? isActive,
        CancellationToken ct)
    {
        var sql = "EXEC dbo.sp_users_get_paged @PageNumber, @PageSize, @Search, @IsActive";

        SqlParameter[] parameters =
        [
            new SqlParameter("@PageNumber", pageNumber),
        new SqlParameter("@PageSize", pageSize),
        new SqlParameter("@Search", (object?)search ?? DBNull.Value),
        new SqlParameter("@IsActive", (object?)isActive ?? DBNull.Value),
    ];

        var rows = await _ctx.UsersList
            .FromSqlRaw(sql, parameters)
            .ToListAsync(ct);

        var total = rows.FirstOrDefault()?.TotalCount ?? 0;

        return new PagedResultQueryModel<UserListItemQueryModel>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = total,
            Items = rows
        };
    }
}