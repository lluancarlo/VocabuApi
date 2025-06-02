using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Common;
using Vocabu.API.Features.Common;
using Vocabu.API.Features.Player;

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

    //[HttpGet("GetArticles")]
    //public async Task<IActionResult> GetArticlesAsync(GetPrepositionsQuery query)
    //{
    //    var commandResult = await _mediator.Send(query);
    //    return StatusCode(commandResult.StatusCode, commandResult);
    //}

    [HttpGet("GetPrepositions")]
    public async Task<IActionResult> GetPrepositionsAsync([FromQuery] GetPrepositionsQuery query)
    {
        var commandResult = await _mediator.Send(query);
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
