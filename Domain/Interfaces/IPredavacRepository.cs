using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Interfaces;

public interface IPredavacRepository
    : IGenericRepository<Predavac>
{
    Task<List<Predaje>> GetZaPredmet(
        int predmetId);

    Task<List<Predaje>> GetPredmete(
        int predavacId);

    Task<Predavac?> GetByIdentityId(
        string identityId);
}