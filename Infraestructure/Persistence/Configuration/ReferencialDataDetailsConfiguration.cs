using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Configuration;

public class ReferencialDataDetailsConfiguration : IEntityTypeConfiguration<ReferencialDataDetailsEntity>
{
    public void Configure(EntityTypeBuilder<ReferencialDataDetailsEntity> builder)
    {
        builder.ToTable("TB_ReferencialData_Details");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(e => e.IdReferencialData)
            .HasColumnName("IdReferencialData")
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("Description")
            .HasMaxLength(200)
            .IsRequired();
    }
}