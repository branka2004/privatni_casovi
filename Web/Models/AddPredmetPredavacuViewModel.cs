namespace Web.Models;

public class AddPredmetPredavacuViewModel
{
    public int PredmetId { get; set; }

    public int GodineIskustva { get; set; }

    public int Nivo { get; set; }

    public bool NoviPredmet { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Oblast { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();



    public List<PredmetDto> Predmeti { get; set; }
        = new();


}