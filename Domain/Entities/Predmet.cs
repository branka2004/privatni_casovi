using PrivateLessons.Domain.Entities;

namespace PrivateLessons.Domain.Entities;

public class Predmet
{
    public int PredmetId { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Oblast { get; set; } = string.Empty;

    public ICollection<Cas> Casovi { get; set; }
        = new List<Cas>();

    public ICollection<Predaje> Predavaci { get; set; }
        = new List<Predaje>();
}