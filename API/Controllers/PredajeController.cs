using API.Features.Predaje.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateLessons.Application.DTOs;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredajeController : ControllerBase
{
    private readonly ISender _mediator;

    public PredajeController(
    ISender mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Predavac")]
    [HttpPost]
    public async Task<IActionResult> Create(
     CreatePredajeDto dto)
    {
        var result =
            await _mediator.Send(
                new CreatePredajeCommand(
                    dto.PredavacId,
                    dto.PredmetId,
                    dto.GodineIskustva,
                    dto.Nivo));

        if (!result)
        {
            return BadRequest(
                "Predavač već predaje ovaj predmet.");
        }

        return Ok();
    }

    [Authorize(Roles = "Predavac")]
    [HttpPost("novi-predmet")]
    public async Task<IActionResult> NoviPredmet(
    CreateNoviPredmetDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Naziv))
            return BadRequest("Naziv predmeta je obavezan.");

        if (string.IsNullOrWhiteSpace(dto.Oblast))
            return BadRequest("Oblast je obavezna.");

        if (dto.GodineIskustva <= 0)
            return BadRequest(
                "Godine iskustva moraju biti veće od 0.");

        var result =
            await _mediator.Send(
                new CreateNoviPredmetCommand(
                    dto.PredavacId,
                    dto.Naziv,
                    dto.Oblast,
                    dto.GodineIskustva,
                    dto.Nivo));

        return Ok(result);
    }

    [Authorize(Roles = "Predavac")]
    [HttpDelete("{predavacId}/{predmetId}")]
    public async Task<IActionResult> Delete(
    int predavacId,
    int predmetId)
    {
        var result =
            await _mediator.Send(
                new DeletePredajeCommand(
                    predavacId,
                    predmetId));

        if (!result)
            return NotFound();

        return NoContent();
    }


}


