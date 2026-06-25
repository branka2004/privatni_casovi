using Microsoft.EntityFrameworkCore;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Enums;
using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Data;

namespace PrivateLessons.Infrastructure.Repositories;

public class CasRepository
    : GenericRepository<Cas>,
      ICasRepository
{
    public CasRepository(
        PrivateLessonsDbContext context)
        : base(context)
    {
    }

    public async Task<bool> PostojiTermin(
        int predavacId,
        DateTime datum,
        TimeSpan pocetak,
        TimeSpan kraj)
    {
        return await _context.Casovi.AnyAsync(x =>
            x.PredavacId == predavacId &&
            x.Datum.Date == datum.Date &&
            x.Status != StatusCasa.Otkazan &&
            pocetak < x.VremeZavrsetka &&
            kraj > x.VremePocetka);
    }

    public async Task<Cas?> GetByIdSaDetaljima(
        int id)
    {
        return await _context.Casovi
            .Include(x => x.Ucenik)
            .Include(x => x.Predavac)
            .Include(x => x.Predmet)
            .FirstOrDefaultAsync(x =>
                x.CasId == id);
    }

    public async Task<List<Cas>> GetZaPredavaca(
        int predavacId)
    {
        return await _context.Casovi
            .Include(x => x.Ucenik)
            .Include(x => x.Predavac)
            .Include(x => x.Predmet)
            .Where(x =>
                x.PredavacId == predavacId)
            .ToListAsync();
    }

    public async Task<List<Cas>> GetZaUcenika(
        int ucenikId)
    {
        return await _context.Casovi
            .Include(x => x.Ucenik)
            .Include(x => x.Predavac)
            .Include(x => x.Predmet)
            .Where(x =>
                x.UcenikId == ucenikId)
            .ToListAsync();
    }

    public async Task<List<Cas>> GetSveSaDetaljima()
    {
        return await _context.Casovi
            .Include(x => x.Ucenik)
            .Include(x => x.Predavac)
            .Include(x => x.Predmet)
            .ToListAsync();
    }

    public async Task<bool> PostojiCasZaPredmet(
    int predavacId,
    int predmetId)
    {
        return await _context.Casovi.AnyAsync(x =>
            x.PredavacId == predavacId &&
            x.PredmetId == predmetId &&
            x.Status != StatusCasa.Otkazan);
    }


}