namespace Web.Models;

public class ZakaziCasViewModel
{
    public int PredavacId { get; set; }

    public int PredmetId { get; set; }

    public DateTime Datum { get; set; }

    public TimeSpan VremePocetka { get; set; }

    public TimeSpan VremeZavrsetka { get; set; }
    public List<string> Errors { get; set; } = new();
}