using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Data;
using PrivateLessons.Infrastructure.Repositories;

namespace PrivateLessons.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly PrivateLessonsDbContext _context;

    public UnitOfWork(
        PrivateLessonsDbContext context)
    {
        _context = context;

        Ucenici =
            new UcenikRepository(_context);

        Predavaci =
            new PredavacRepository(_context);

        Predmeti =
            new PredmetRepository(_context);

        Casovi =
            new CasRepository(_context);

        Predaje =
            new PredajeRepository(_context);
    }

    public IUcenikRepository Ucenici { get; }

    public IPredavacRepository Predavaci { get; }

    public IPredmetRepository Predmeti { get; }

    public ICasRepository Casovi { get; }

    public IPredajeRepository Predaje { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}