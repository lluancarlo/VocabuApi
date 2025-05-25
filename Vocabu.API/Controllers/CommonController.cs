using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Common;
using Vocabu.API.Features.Common;

namespace Vocabu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommonController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetApiStatus")]
    public IActionResult GetApiStatus()
    {
        var commandResult = ApiResponse.Ok("API online and ready to be used!");
        return StatusCode(commandResult.StatusCode, commandResult);
    }

    [HttpGet("GetCountries")]
    public async Task<IActionResult> GetCountriesAsync()
    {
        var commandResult = await _mediator.Send(new GetAllCountriesQuery());
        return StatusCode(commandResult.StatusCode, commandResult);
    }

    [HttpGet("GetWord")]
    public async Task<IActionResult> GetWordAsync([FromQuery] GetWordQuery query)
    {
        var commandResult = await _mediator.Send(query);
        return StatusCode(commandResult.StatusCode, commandResult);
    }
}
