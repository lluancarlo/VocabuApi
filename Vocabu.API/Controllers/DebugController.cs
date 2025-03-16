using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Vocabu.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DebugController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("GetHelloWorld")]
    public IActionResult GetHelloWorld() => Ok("Hello World!");

    [HttpGet("GetAppSettings")]
    public IActionResult GetAppSettings()
    {
        var jwtConfig = _configuration.GetSection("Jwt");
        string json = JsonSerializer.Serialize(new
        {
            Secret = jwtConfig.GetSection("Secret").Value,
            Issuer = jwtConfig.GetSection("Issuer").Value,
            Audience = jwtConfig.GetSection("Audience").Value
        });
        return Content(json, "application/json");
    }
}
