using PrivateLessons.Domain.Enums;

namespace PrivateLessons.Domain.Entities;

public class Cas
{
    public int CasId { get; set; }

    public DateTime Datum { get; set; }

    public TimeSpan VremePocetka { get; set; }

    public TimeSpan VremeZavrsetka { get; set; }

    public StatusCasa Status { get; set; }

    public int UcenikId { get; set; }

    public Ucenik Ucenik { get; set; } = null!;

    public int PredavacId { get; set; }

    public Predavac Predavac { get; set; } = null!;

    public int PredmetId { get; set; }

    public Predmet Predmet { get; set; } = null!;
}