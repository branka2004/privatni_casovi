using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Interfaces;

public interface IPredajeRepository
    : IGenericRepository<Predaje>
{
    Task<bool> PostojiPredavanje(
        int predavacId,
        int predmetId);

    Task<Predaje?> GetPredavanje(
        int predavacId,
        int predmetId);
}