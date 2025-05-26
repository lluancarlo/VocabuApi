using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Common;
using Vocabu.API.Features.Common;

namespace Vocabu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetPrepositions")]
    public async Task<IActionResult> GetPrepositionsAsync()
    {
        var commandResult = await _mediator.Send(new GetAllCountriesQuery());
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
