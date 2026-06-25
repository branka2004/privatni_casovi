namespace PrivateLessons.Application.DTOs;

public class CreatePredavacDto
{
    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Lozinka { get; set; } = string.Empty;

    public string Biografija { get; set; } = string.Empty;

    public decimal CenaPoSatu { get; set; }
}