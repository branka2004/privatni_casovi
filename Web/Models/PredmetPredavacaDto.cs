namespace Web.Models;

public class PredmetPredavacaDto
{
    public int PredmetId { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Oblast { get; set; } = string.Empty;

    public int GodineIskustva { get; set; }

    public string Nivo { get; set; } = string.Empty;
}