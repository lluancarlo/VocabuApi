using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Vocabu.API.Common;

namespace Vocabu.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ConfigurationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("GetStatus")]
    public IActionResult GetHelloWorld()
    {
        var commandResult = CommandResponse.Ok("API online and ready to use!");
        return StatusCode(commandResult.StatusCode, commandResult);
    }

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
