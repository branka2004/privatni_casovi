namespace PrivateLessons.Application.DTOs;

public class UcenikDto
{
    public int KorisnikId { get; set; }

    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Skola { get; set; } = string.Empty;

    public int Razred { get; set; }
}