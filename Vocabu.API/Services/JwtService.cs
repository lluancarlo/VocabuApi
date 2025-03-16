using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VocabuApi.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> GenerateToken(string email)
    {
        var appSettingsKey = _configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(appSettingsKey))
            throw new MissingAppSettingsConfigurationException();

        var keyInBytes = Encoding.UTF8.GetBytes(appSettingsKey);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: new List<Claim> { new Claim(ClaimTypes.Email, email) },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyInBytes), SecurityAlgorithms.HmacSha256)
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}

class MissingAppSettingsConfigurationException() : Exception();