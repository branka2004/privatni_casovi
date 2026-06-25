using Microsoft.AspNetCore.Mvc;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UceniciController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UceniciController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UcenikDto>>> GetAll()
    {
        var ucenici = await _unitOfWork.Ucenici.GetAllAsync();

        var result = ucenici.Select(x => new UcenikDto
        {
            KorisnikId = x.KorisnikId,
            Ime = x.Ime,
            Prezime = x.Prezime,
            Email = x.Email,
            Skola = x.Skola,
            Razred = x.Razred
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateUcenikDto dto)
    {
        var ucenik = new Ucenik
        {
            Ime = dto.Ime,
            Prezime = dto.Prezime,
            Email = dto.Email,
            DatumRegistracije = DateTime.UtcNow,
            Skola = dto.Skola,
            Razred = dto.Razred
        };

        await _unitOfWork.Ucenici.AddAsync(ucenik);

        await _unitOfWork.SaveChangesAsync();

        return Ok(ucenik);
    }
}