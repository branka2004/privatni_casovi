using API.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUser.LoginUserCommand command)
    {
        var result =
            await _mediator.Send(command);

        if (result == null)
        {
            return Unauthorized(
                new
                {
                    message =
                        "Neispravan email ili lozinka."
                });
        }

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
    [FromBody] RegisterUserCommand command)
    {
        var result =
            await _mediator.Send(command);

        if (result == null || !result.Succeeded)
        {
            return BadRequest(
                new
                {
                    message =
                        result?.Message ??
                        "Greška prilikom registracije.",

                    errors =
                        result?.Errors
                });
        }

        return Ok(result);
    }
}