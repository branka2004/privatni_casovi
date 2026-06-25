using API.Features.Casovi.Commands;
using API.Features.Casovi.Queries;
    using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using PrivateLessons.Application.DTOs;
    using PrivateLessons.Domain.Entities;
    using PrivateLessons.Domain.Enums;
    using PrivateLessons.Domain.Interfaces;
    using PrivateLessons.Infrastructure.Data;


namespace API.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class CasoviController : ControllerBase
    {
        
    private readonly ISender _mediator;

    public CasoviController(
     ISender mediator)
    {
        
        _mediator = mediator;
    }

    [Authorize(Roles = "Ucenik")]
    [HttpPost]
    public async Task<ActionResult> ZakaziCas(
[FromBody] CreateCasDto dto)
    {
        if (dto.VremePocetka == dto.VremeZavrsetka)
        {
            return BadRequest(
            "Vreme početka i završetka ne sme biti isto.");
        }


if (dto.VremePocetka > dto.VremeZavrsetka)
        {
            return BadRequest(
                "Vreme završetka mora biti kasnije od vremena početka.");
        }

        var cas =
            await _mediator.Send(
                new CreateCasCommand(
                    dto.UcenikId,
                    dto.PredavacId,
                    dto.PredmetId,
                    dto.Datum,
                    dto.VremePocetka,
                    dto.VremeZavrsetka));

        if (cas == null)
        {
            return BadRequest(
                "Predavač već ima zakazan čas u tom terminu.");
        }

        return Ok(cas);


}

    [Authorize(Roles = "Predavac, Ucenik")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CasDto>>> GetAll()
    {
        var result =
            await _mediator.Send(
                new GetAllCasoviQuery());

        return Ok(result);
    }
    [Authorize(Roles = "Ucenik,Predavac")]
    [HttpPut("otkazi/{id}")]
    public async Task<IActionResult> OtkaziCas(
    int id,
    string otkazao)
    {
        var result =
            await _mediator.Send(
                new OtkaziCasCommand(
                    id,
                    otkazao));

        if (!result)
            return NotFound();

        return Ok();
    }
    [Authorize(Roles = "Ucenik")]
    [HttpGet("ucenik/{ucenikId}")]
    public async Task<ActionResult<IEnumerable<CasDto>>>
GetZaUcenika(int ucenikId)
    {
        var result =
            await _mediator.Send(
                new GetCasoviZaUcenikaQuery(
                    ucenikId));

        return Ok(result);
    }
    [Authorize(Roles = "Predavac")]
    [HttpGet("predavac/{predavacId}")]
    public async Task<ActionResult<IEnumerable<CasDto>>>
GetZaPredavaca(int predavacId)
    {
        var result =
            await _mediator.Send(
                new GetCasoviZaPredavacaQuery(
                    predavacId));

        return Ok(result);
    }

    [Authorize(Roles = "Predavac")]
    [HttpPut("odrzan/{id}")]
    public async Task<IActionResult> OznaciKaoOdrzan(
        int id)
    {
        var result =
            await _mediator.Send(
                new OznaciKaoOdrzanCommand(id));

        if (!result)
            return NotFound();

        return Ok();
    }
    [Authorize(Roles = "Predavac")]
    [HttpPut("potvrdi/{id}")]
    public async Task<IActionResult> Potvrdi(
    int id)
    {
        var result =
            await _mediator.Send(
                new PotvrdiCasCommand(id));

        if (!result)
            return NotFound();

        return Ok();
    }

    [Authorize(Roles = "Predavac")]
    [HttpPut("odbij/{id}")]
    public async Task<IActionResult> Odbij(
        int id)
    {
        var result =
            await _mediator.Send(
                new OdbijCasCommand(id));

        if (!result)
            return NotFound();

        return Ok();
    }
}