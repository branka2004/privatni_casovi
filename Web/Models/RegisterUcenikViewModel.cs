namespace Web.Models;

public class RegisterUcenikViewModel
{
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Lozinka { get; set; } = string.Empty;
    public string Skola { get; set; } = string.Empty;
    public int Razred { get; set; }
    public List<string> Errors { get; set; } = new();
}