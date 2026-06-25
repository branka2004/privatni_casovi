using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Interfaces;

public interface IUnitOfWork
{
    IUcenikRepository Ucenici { get; }

    IPredavacRepository Predavaci { get; }

    IPredmetRepository Predmeti { get; }

    ICasRepository Casovi { get; }

    IPredajeRepository Predaje { get; }

    Task<int> SaveChangesAsync();
}