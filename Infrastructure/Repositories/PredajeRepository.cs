using Microsoft.EntityFrameworkCore;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Data;

namespace PrivateLessons.Infrastructure.Repositories;

public class PredajeRepository
    : GenericRepository<Predaje>,
      IPredajeRepository
{
    public PredajeRepository(
        PrivateLessonsDbContext context)
        : base(context)
    {
    }

    public async Task<bool> PostojiPredavanje(
        int predavacId,
        int predmetId)
    {
        return await _dbSet.AnyAsync(x =>
            x.PredavacId == predavacId &&
            x.PredmetId == predmetId);
    }

    public async Task<Predaje?> GetPredavanje(
        int predavacId,
        int predmetId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x =>
                x.PredavacId == predavacId &&
                x.PredmetId == predmetId);
    }
}