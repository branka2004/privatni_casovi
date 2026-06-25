using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Infrastructure.Configurations;

public class PredmetConfiguration : IEntityTypeConfiguration<Predmet>
{
    public void Configure(EntityTypeBuilder<Predmet> builder)
    {
        builder.HasKey(x => x.PredmetId);

        builder.Property(x => x.Naziv)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Oblast)
            .IsRequired()
            .HasMaxLength(100);
    }
}