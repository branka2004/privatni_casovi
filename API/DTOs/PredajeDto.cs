namespace PrivateLessons.Application.DTOs;

public class PredajeDto
{
    public int PredavacId { get; set; }

    public string PredavacIme { get; set; } = string.Empty;

    public int PredmetId { get; set; }

    public string PredmetNaziv { get; set; } = string.Empty;
}