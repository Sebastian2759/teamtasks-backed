using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configuration;

public class MasterDataDetailConfiguration : IEntityTypeConfiguration<MasterDataDetailEntity>
{
    public void Configure(EntityTypeBuilder<MasterDataDetailEntity> b)
    {
        b.ToTable("MasterDataDetail");
        b.HasKey(x => x.Id);

        b.Property(x => x.Name).HasMaxLength(120).IsRequired();
        b.HasIndex(x => new { x.MasterDataId, x.Name }).IsUnique();
    }
}
