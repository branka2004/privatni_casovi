using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Infrastructure.Identity;

namespace PrivateLessons.Infrastructure.Data;

public class PrivateLessonsDbContext
    : IdentityDbContext<ApplicationUser>
{
    public PrivateLessonsDbContext(
        DbContextOptions<PrivateLessonsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ucenik> Ucenici => Set<Ucenik>();

    public DbSet<Predavac> Predavaci => Set<Predavac>();

    public DbSet<Predmet> Predmeti => Set<Predmet>();

    public DbSet<Cas> Casovi => Set<Cas>();

    public DbSet<Predaje> Predaje => Set<Predaje>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PrivateLessonsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}