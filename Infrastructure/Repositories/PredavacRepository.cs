using Microsoft.EntityFrameworkCore;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Data;

namespace PrivateLessons.Infrastructure.Repositories;

public class PredavacRepository
    : GenericRepository<Predavac>,
      IPredavacRepository
{
    public PredavacRepository(
        PrivateLessonsDbContext context)
        : base(context)
    {
    }

    public async Task<List<Predaje>> GetZaPredmet(
    int predmetId)
    {
        return await _context.Predaje
            .Include(x => x.Predavac)
            .Where(x => x.PredmetId == predmetId)
            .ToListAsync();
    }

    public async Task<List<Predaje>> GetPredmete(
        int predavacId)
    {
        return await _context.Predaje
            .Include(x => x.Predmet)
            .Where(x =>
                x.PredavacId == predavacId)
            .ToListAsync();
    }

    public async Task<Predavac?> GetByIdentityId(
        string identityId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x =>
                x.IdentityUserId == identityId);
    }
}