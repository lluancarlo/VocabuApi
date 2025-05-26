using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Common;
using Vocabu.API.Features.Common;
using Vocabu.API.Features.Player;
using Vocabu.API.Features.Profile;

namespace Vocabu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetPlayer")]
    public async Task<IActionResult> GetPlayerAsync(GetPlayerQuery query)
    {
        var commandResult = await _mediator.Send(query);
        return StatusCode(commandResult.StatusCode, commandResult);
    }

    [HttpPost("AddPlayerExperience")]
    public async Task<IActionResult> AddPlayerExperience(AddPlayerExperienceCommand command)
    {
        var commandResult = await _mediator.Send(command);
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
