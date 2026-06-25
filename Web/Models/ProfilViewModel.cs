namespace Web.Models;

public class ProfilViewModel
{
    public PredavacDto Predavac { get; set; }
        = new();

    public List<CasDto> Casovi { get; set; }
        = new();
}