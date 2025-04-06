using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Features.Auth;

namespace Vocabu.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScoreboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScoreboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("AddPoints")]
    public async Task<IActionResult> AddPoints([FromBody] AddPointsCommand command)
    {
        var commandResult = await _mediator.Send(command);
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
