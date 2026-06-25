namespace PrivateLessons.Application.DTOs;

public class CreateNoviPredmetDto
{
    public int PredavacId { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Oblast { get; set; } = string.Empty;

    public int GodineIskustva { get; set; }

    public int Nivo { get; set; }
}