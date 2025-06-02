using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Vocabu.BL;
using Vocabu.DAL;
using Vocabu.DAL.Contexts;
using Vocabu.DAL.Entities;

namespace Vocabu.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load Layers
        BusinessLayerExtension.LoadServices(builder.Services);
        DataAccessLayerExtension.LoadServices<ApiDbContext>(builder.Services, builder.Configuration);

        // Configure services
        ConfiguraAspNetCore(builder.Services, builder.Logging);
        ConfigureSwaggerService(builder.Services);
        ConfigureJwtToken(builder.Services, builder.Configuration);
        ConfigureAspNetCoreIdentity(builder.Services);
        ConfigureSerilog(builder.Configuration, builder.Host);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
        }
        app.UseOpenApi();
        app.UseSwaggerUi();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static void ConfiguraAspNetCore(IServiceCollection appServices, ILoggingBuilder appDefaultLogging)
    {
        appServices.AddControllers();
        appServices.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        appServices.AddValidatorsFromAssembly(typeof(Program).Assembly);
        appDefaultLogging.ClearProviders();
    }

    private static void ConfigureSwaggerService(IServiceCollection appServices)
    {
        appServices.AddOpenApiDocument(options =>
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
    }

    private static void ConfigureJwtToken(IServiceCollection appServices, IConfiguration appConfiguration)
    {
        var key = System.Text.Encoding.UTF8.GetBytes(appConfiguration["Jwt:Secret"]!);
        appServices
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
                    ValidIssuer = appConfiguration["Jwt:Issuer"],
                    ValidAudience = appConfiguration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
    }

    private static void ConfigureAspNetCoreIdentity(IServiceCollection appServices)
    {
        appServices.AddIdentity<User, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<ApiDbContext>()
                    .AddDefaultTokenProviders();
        appServices.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });
    }

    private static void ConfigureSerilog(IConfiguration appConfiguration, IHostBuilder apphost)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(appConfiguration)
            .CreateLogger(); ;

        apphost.UseSerilog();
    }
}
