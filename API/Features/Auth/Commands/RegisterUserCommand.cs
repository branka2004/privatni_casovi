using MediatR;
using Microsoft.AspNetCore.Identity;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;
using PrivateLessons.Infrastructure.Identity;

namespace API.Features.Auth.Commands;

public record RegisterUserCommand(
    string Ime,
    string Prezime,
    string Email,
    string Password,
    string Role,
    string? Biografija,
    decimal? CenaPoSatu,
    string? Skola,
    int? Razred)
    : IRequest<RegisterResultDto?>;

public class RegisterResultDto
{
    public bool Succeeded { get; set; }

    public string Message { get; set; } = string.Empty;

    public IEnumerable<string> Errors { get; set; }
        = Enumerable.Empty<string>();
}

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, RegisterResultDto?>
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterResultDto?> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _roleManager.RoleExistsAsync(
            request.Role))
        {
            await _roleManager.CreateAsync(
                new IdentityRole(request.Role));
        }

        var existingUser =
            await _userManager.FindByEmailAsync(
                request.Email);

        if (existingUser != null)
        {
            return new RegisterResultDto
            {
                Succeeded = false,
                Message = "Korisnik već postoji."
            };
        }

        var user =
            new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

        var result =
            await _userManager.CreateAsync(
                user,
                request.Password);

        if (!result.Succeeded)
        {
            return new RegisterResultDto
            {
                Succeeded = false,
                Message = "Registracija nije uspela.",
                Errors = result.Errors
                    .Select(x => x.Description)
            };
        }

        if (request.Role == "Predavac")
        {
            var predavac = new Predavac
            {
                IdentityUserId = user.Id,
                Ime = request.Ime,
                Prezime = request.Prezime,
                Email = request.Email,
                DatumRegistracije = DateTime.UtcNow,
                Biografija = request.Biografija ?? "",
                CenaPoSatu = request.CenaPoSatu ?? 0
            };

            await _unitOfWork.Predavaci.AddAsync(
                predavac);
        }

        if (request.Role == "Ucenik")
        {
            var ucenik = new Ucenik
            {
                IdentityUserId = user.Id,
                Ime = request.Ime,
                Prezime = request.Prezime,
                Email = request.Email,
                DatumRegistracije = DateTime.UtcNow,
                Skola = request.Skola ?? "",
                Razred = request.Razred ?? 0
            };

            await _unitOfWork.Ucenici.AddAsync(
                ucenik);
        }

        await _unitOfWork.SaveChangesAsync();

        await _userManager.AddToRoleAsync(
            user,
            request.Role);

        return new RegisterResultDto
        {
            Succeeded = true,
            Message =
                $"Uspešno kreiran korisnik sa rolom {request.Role}"
        };
    }
}