using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Features.Auth;

namespace Vocabu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var commandResult = await _mediator.Send(command);
        return StatusCode(commandResult.StatusCode, commandResult);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var commandResult = await _mediator.Send(command);
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
