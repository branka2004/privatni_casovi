namespace PrivateLessons.Application.DTOs;

public class CreateUcenikDto
{
    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Lozinka { get; set; } = string.Empty;

    public string Skola { get; set; } = string.Empty;

    public int Razred { get; set; }
}