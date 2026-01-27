using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configuration;

public class ReferencialDataConfiguration : IEntityTypeConfiguration<ReferencialDataEntity>
{
    public void Configure(EntityTypeBuilder<ReferencialDataEntity> builder)
    {
        builder.ToTable("TB_ReferencialData");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .IsRequired();
    }
}