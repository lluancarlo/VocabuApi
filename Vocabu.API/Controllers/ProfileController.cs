using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Features.Profile;

namespace Vocabu.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePlayerCommand command)
    {
        var commandResult = await _mediator.Send(command);
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
