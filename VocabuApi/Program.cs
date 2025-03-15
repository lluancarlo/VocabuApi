using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Vocabu.DataAccessLayer;
using VocabuApi.Services;

namespace VocabuApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DataAccessLayerExtension.LoadServices(builder.Services, builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddOpenApiDocument(options =>
        {
            options.DocumentName = "Vocabu Api";
            options.Version = "v1";
            options.AddSecurity("Bearer", new OpenApiSecurityScheme
            {
                Description = "Bearer token authorization header",
                Type = OpenApiSecuritySchemeType.Http,
                In = OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer"
            });
            options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // JWT Token
        builder.Services.AddScoped<JwtService>();
        var key = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!);
        builder.Services
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
        }

        app.UseOpenApi();
        app.UseSwaggerUi();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
