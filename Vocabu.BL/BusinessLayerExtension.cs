using Microsoft.Extensions.DependencyInjection;
using Vocabu.BL.Services;

namespace Vocabu.BL;

public static class BusinessLayerExtension
{
    public static void LoadServices(IServiceCollection serviceProvider)
    {
        serviceProvider.AddScoped<JwtService>();
    }
}
