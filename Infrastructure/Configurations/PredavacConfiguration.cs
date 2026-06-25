using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Infrastructure.Configurations;

public class PredavacConfiguration : IEntityTypeConfiguration<Predavac>
{
    public void Configure(EntityTypeBuilder<Predavac> builder)
    {
        builder.Property(x => x.Biografija)
            .HasMaxLength(1000);

        builder.Property(x => x.CenaPoSatu)
            .HasPrecision(18, 2);
    }
}