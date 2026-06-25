using Microsoft.EntityFrameworkCore;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Data;

namespace PrivateLessons.Infrastructure.Repositories;

public class UcenikRepository
    : GenericRepository<Ucenik>,
      IUcenikRepository
{
    public UcenikRepository(
        PrivateLessonsDbContext context)
        : base(context)
    {
    }

    public async Task<Ucenik?> GetByIdentityId(
        string identityId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x =>
                x.IdentityUserId == identityId);
    }
}