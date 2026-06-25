using PrivateLessons.Domain.Enums;

namespace PrivateLessons.Domain.Entities;

public class Predaje
{
    public int PredavacId { get; set; }

    public Predavac Predavac { get; set; } = null!;

    public int PredmetId { get; set; }

    public Predmet Predmet { get; set; } = null!;

    public int GodineIskustva { get; set; }

    public NivoPredmeta Nivo { get; set; }
}