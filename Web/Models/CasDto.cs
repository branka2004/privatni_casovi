namespace Web.Models;

public class CasDto
{
    public int CasId { get; set; }

    public string UcenikIme { get; set; } = string.Empty;

    public string PredavacIme { get; set; } = string.Empty;

    public string PredmetNaziv { get; set; } = string.Empty;

    public DateTime Datum { get; set; }

    public TimeSpan VremePocetka { get; set; }

    public TimeSpan VremeZavrsetka { get; set; }

    public string Status { get; set; } = string.Empty;
}