using Microsoft.AspNetCore.Mvc;
using PrivateLessons.Application.DTOs;
using PrivateLessons.Domain.Entities;
using PrivateLessons.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PrivateLessons.Infrastructure.Data;
using MediatR;
using API.Features.Predmeti.Commands;
using API.Features.Predmeti.Queries;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredmetiController : ControllerBase
{
    private readonly ISender _mediator;

    public PredmetiController(
     ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PredmetDto>>> GetAll()
    {
        var result =
     await _mediator.Send(
         new GetAllPredmetiQuery());

        return Ok(result);

        

    }



    [Authorize(Roles = "Predavac")]
    [HttpPost]
    public async Task<ActionResult> Create(CreatePredmetDto dto)
    {
        var id =
    await _mediator.Send(
        new CreatePredmetCommand
        {
            Naziv = dto.Naziv,
            Oblast = dto.Oblast
        });

        return Ok(id);
    }


    [Authorize(Roles = "Predavac")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    int id,
    UpdatePredmetDto dto)
    {
        await _mediator.Send(
     new UpdatePredmetCommand(
         id,
         dto.Naziv,
         dto.Oblast));

        return Ok();
    }
    [Authorize(Roles = "Predavac")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(
    new DeletePredmetCommand(id));

        return NoContent();
            
    }

    [HttpGet("predavac/{predavacId}")]
    public async Task<ActionResult<IEnumerable<PredmetDto>>>
 GetPredmetiZaPredavaca(int predavacId)
    {
        var result =
            await _mediator.Send(
                new GetPredmetiZaPredavacaQuery(
                    predavacId));

        return Ok(result);
    }

    [HttpGet("dostupni/{predavacId}")]
    public async Task<ActionResult<IEnumerable<PredmetDto>>>
GetDostupniPredmeti(int predavacId)
    {
        var result =
            await _mediator.Send(
                new GetDostupniPredmetiQuery(
                    predavacId));

        return Ok(result);
    }
}