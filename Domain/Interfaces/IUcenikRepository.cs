using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Interfaces;

public interface IUcenikRepository
    : IGenericRepository<Ucenik>
{
    Task<Ucenik?> GetByIdentityId(
        string identityId);
}