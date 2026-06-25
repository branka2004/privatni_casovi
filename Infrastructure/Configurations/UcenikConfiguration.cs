using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Infrastructure.Configurations;

public class UcenikConfiguration : IEntityTypeConfiguration<Ucenik>
{
    public void Configure(EntityTypeBuilder<Ucenik> builder)
    {
        builder.Property(x => x.Razred)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Skola)
            .IsRequired()
            .HasMaxLength(100);
    }
}