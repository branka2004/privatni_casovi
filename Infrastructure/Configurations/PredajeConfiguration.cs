using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Infrastructure.Configurations;

public class PredajeConfiguration : IEntityTypeConfiguration<Predaje>
{
    public void Configure(EntityTypeBuilder<Predaje> builder)
    {
        builder.HasKey(x => new
        {
            x.PredavacId,
            x.PredmetId
        });

        builder.HasOne(x => x.Predavac)
            .WithMany(x => x.PredajePredmete)
            .HasForeignKey(x => x.PredavacId);

        builder.HasOne(x => x.Predmet)
            .WithMany(x => x.Predavaci)
            .HasForeignKey(x => x.PredmetId);
    }
}