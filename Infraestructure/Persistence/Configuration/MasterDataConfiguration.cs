using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class MasterDataConfiguration : IEntityTypeConfiguration<MasterDataEntity>
    {
        public void Configure(EntityTypeBuilder<MasterDataEntity> b)
        {
            b.ToTable("MasterData");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name).HasMaxLength(120).IsRequired();
            b.HasIndex(x => x.Name).IsUnique();

        }
    }
}
