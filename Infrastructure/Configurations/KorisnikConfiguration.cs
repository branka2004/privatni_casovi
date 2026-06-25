using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Infrastructure.Configurations;

public class KorisnikConfiguration : IEntityTypeConfiguration<Korisnik>
{
    public void Configure(EntityTypeBuilder<Korisnik> builder)
    {
        builder.HasKey(x => x.KorisnikId);

        builder.Property(x => x.Ime)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Prezime)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        

        builder.HasDiscriminator<string>("TipKorisnika")
            .HasValue<Ucenik>("UCENIK")
            .HasValue<Predavac>("PREDAVAC");
    }
}