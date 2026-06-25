using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Data;

namespace PrivateLessons.Infrastructure.Repositories;

public class PredmetRepository
    : GenericRepository<Predmet>,
      IPredmetRepository
{
    public PredmetRepository(
        PrivateLessonsDbContext context)
        : base(context)
    {
    }
}