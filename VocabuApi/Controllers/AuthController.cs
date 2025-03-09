using MediatR;
using Microsoft.AspNetCore.Mvc;
using VocabuApi.Common;
using VocabuApi.Features.Auth;

[ApiController]
[Route("api/auth")]
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
