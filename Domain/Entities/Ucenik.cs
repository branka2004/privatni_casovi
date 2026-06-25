using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Entities;

public class Ucenik : Korisnik
{
    public int Razred { get; set; }

    public string Skola { get; set; } = string.Empty;

    public ICollection<Cas> Casovi { get; set; }
        = new List<Cas>();
}