    using API.Features.Predavaci.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using PrivateLessons.Application.DTOs;
    using API.Features.Predavaci.Commands;

    namespace API.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class PredavaciController : ControllerBase
    {
        private readonly ISender _mediator;


    public PredavaciController(
        ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredavacDto>>> GetAll()
        {
            var result =
                await _mediator.Send(
                    new GetAllPredavaciQuery());

            return Ok(result);
        }

        [HttpGet("predmet/{predmetId}")]
        public async Task<IActionResult> GetZaPredmet(
            int predmetId)
        {
            var result =
                await _mediator.Send(
                    new GetPredavaciZaPredmetQuery(
                        predmetId));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PredavacDto>> GetById(
            int id)
        {
            var result =
                await _mediator.Send(
                    new GetPredavacByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfil(
            int id,
            UpdateProfilPredavacaDto dto)
        {
            var result =
                await _mediator.Send(
                    new UpdateProfilPredavacaCommand(
                        id,
                        dto.Biografija,
                        dto.CenaPoSatu));

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpGet("{id}/predmeti")]
        public async Task<IActionResult> GetPredmeti(
        int id)
        {
            var result =
                await _mediator.Send(
                    new GetPredmetiPredavacaQuery(id));

            return Ok(result);
        }


    }
