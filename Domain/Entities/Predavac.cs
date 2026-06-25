using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Entities;

public class Predavac : Korisnik
{
    public string Biografija { get; set; } = string.Empty;

    public decimal CenaPoSatu { get; set; }

    public ICollection<Cas> Casovi { get; set; }
        = new List<Cas>();

    public ICollection<Predaje> PredajePredmete { get; set; }
        = new List<Predaje>();
}