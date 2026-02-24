using Domain.Entities;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Persistence.Configuration;

namespace Persistence.Context;

public sealed class TeamTasksSampleContext : DbContext
{
    public TeamTasksSampleContext(DbContextOptions<TeamTasksSampleContext> options) : base(options){}
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<MasterDataEntity> MasterData => Set<MasterDataEntity>();
    public DbSet<MasterDataDetailEntity> MasterDataDetail => Set<MasterDataDetailEntity>();
    public DbSet<UserListItemQueryModel> UsersList => Set<UserListItemQueryModel>();
    public DbSet<TaskListItemQueryModel> TasksList => Set<TaskListItemQueryModel>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskConfiguration).Assembly);
        modelBuilder.Entity<UserListItemQueryModel>().HasNoKey();
        modelBuilder.Entity<TaskListItemQueryModel>().HasNoKey();
    }

    internal void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }
}