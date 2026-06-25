using Microsoft.IdentityModel.Tokens;
using PrivateLessons.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Service;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(
     ApplicationUser user,
     IList<string> roles,
     int korisnikId)
    {
        var claims = new List<Claim>
{
    new Claim(
        ClaimTypes.NameIdentifier,
        user.Id),

    new Claim(
        ClaimTypes.Email,
        user.Email ?? ""),

    new Claim(
        ClaimTypes.Name,
        user.UserName ?? ""),

    new Claim(
        "KorisnikId",
        korisnikId.ToString())
};

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role));
        }

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JwtSettings:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer:
                    _configuration["JwtSettings:Issuer"],
                audience:
                    _configuration["JwtSettings:Audience"],
                claims: claims,
                expires:
                    DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}