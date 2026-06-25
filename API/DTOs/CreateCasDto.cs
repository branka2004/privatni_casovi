namespace PrivateLessons.Application.DTOs;

public class CreateCasDto
{
    public int UcenikId { get; set; }

    public int PredavacId { get; set; }

    public int PredmetId { get; set; }

    public DateTime Datum { get; set; }

    public TimeSpan VremePocetka { get; set; }

    public TimeSpan VremeZavrsetka { get; set; }
}