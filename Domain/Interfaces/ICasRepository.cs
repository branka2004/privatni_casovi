using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Interfaces;

public interface ICasRepository
    : IGenericRepository<Cas>
{
    Task<bool> PostojiTermin(
        int predavacId,
        DateTime datum,
        TimeSpan pocetak,
        TimeSpan kraj);

    Task<List<Cas>> GetZaPredavaca(
        int predavacId);

    Task<List<Cas>> GetZaUcenika(
        int ucenikId);

    Task<List<Cas>> GetSveSaDetaljima();

    Task<Cas?> GetByIdSaDetaljima(
        int id);

    Task<bool> PostojiCasZaPredmet(
    int predavacId,
    int predmetId);
}