public abstract class Korisnik
{
    public int KorisnikId { get; set; }

    public string IdentityUserId { get; set; }
        = string.Empty;

    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DatumRegistracije { get; set; }
}