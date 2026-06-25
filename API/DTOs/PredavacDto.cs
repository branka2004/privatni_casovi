namespace PrivateLessons.Application.DTOs;

public class PredavacDto
{
    public int KorisnikId { get; set; }

    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Biografija { get; set; } = string.Empty;

    public decimal CenaPoSatu { get; set; }

    public int GodineIskustva { get; set; }

    public string Nivo { get; set; } = string.Empty;

    public int PredmetId { get; set; }
}