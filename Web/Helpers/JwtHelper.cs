using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Helpers;

public static class JwtHelper
{
    public static string GetRole(string token)
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwt =
            handler.ReadJwtToken(token);

        return jwt.Claims
            .FirstOrDefault(x =>
                x.Type == ClaimTypes.Role)
            ?.Value ?? "";
    }

    public static int GetKorisnikId(string token)
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwt =
            handler.ReadJwtToken(token);

        var value =
            jwt.Claims
                .FirstOrDefault(x =>
                    x.Type == "KorisnikId")
                ?.Value;

        return int.TryParse(
            value,
            out int id)
            ? id
            : 0;
    }
}