using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vocabu.API.Common;
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
    public async Task<ApiResponse> Login([FromBody] LoginCommand command)
    {
        return await _mediator.Send(command);
    }
}
