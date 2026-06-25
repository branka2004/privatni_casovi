using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Infrastructure.Configurations;

public class CasConfiguration : IEntityTypeConfiguration<Cas>
{
    public void Configure(EntityTypeBuilder<Cas> builder)
    {
        builder.HasKey(x => x.CasId);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasOne(x => x.Ucenik)
            .WithMany(x => x.Casovi)
            .HasForeignKey(x => x.UcenikId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Predavac)
            .WithMany(x => x.Casovi)
            .HasForeignKey(x => x.PredavacId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Predmet)
            .WithMany(x => x.Casovi)
            .HasForeignKey(x => x.PredmetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}