using Domain.Entities;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Persistence.Configuration;

namespace Persistence.Context;

public sealed class TeamTasksSampleContext : DbContext
{

    public TeamTasksSampleContext(DbContextOptions<TeamTasksSampleContext> options) : base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfiguration(new ReferencialDataConfiguration());
        modelBuilder.ApplyConfiguration(new ReferencialDataDetailsConfiguration());
        modelBuilder.Entity<DeveloperEntity>().ToTable("Developers").HasKey(x => x.DeveloperId);
        modelBuilder.Entity<ProjectEntity>().ToTable("Projects").HasKey(x => x.ProjectId);
        modelBuilder.Entity<TaskEntity>().ToTable("Tasks").HasKey(x => x.TaskId);
        modelBuilder.Entity<ReferencialDataEntity>().ToTable("TB_ReferencialData").HasKey(x => x.Id);
        modelBuilder.Entity<ReferencialDataDetailsEntity>().ToTable("TB_ReferencialData_Details").HasKey(x => x.Id);
        modelBuilder.Entity<ProjectTaskPagedRow>().HasNoKey().ToView(null);
        modelBuilder.Entity<CreatedTaskRow>().HasNoKey().ToView(null);
        modelBuilder.Entity<DeveloperDelayRiskRow>().HasNoKey().ToView("VW_DeveloperDelayRisk");
    }

    internal void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }

    public DbSet<ReferencialDataEntity> ReferencialData { get; set; }
    public DbSet<ReferencialDataDetailsEntity> ReferencialDataDetails { get; set; }
    public DbSet<DeveloperEntity> Developers => Set<DeveloperEntity>();
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<ReferencialDataEntity> TB_ReferencialData => Set<ReferencialDataEntity>();
    public DbSet<ReferencialDataDetailsEntity> TB_ReferencialData_Details => Set<ReferencialDataDetailsEntity>();
    public DbSet<ProjectTaskPagedRow> ProjectTaskPagedRows => Set<ProjectTaskPagedRow>();
    public DbSet<CreatedTaskRow> CreatedTaskRows => Set<CreatedTaskRow>();
    public DbSet<DeveloperDelayRiskRow> DeveloperDelayRiskRows => Set<DeveloperDelayRiskRow>();
}