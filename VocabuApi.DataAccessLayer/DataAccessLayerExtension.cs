using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vocabu.DataAccessLayer;

public static class DataAccessLayerExtension
{
    private const string DefaultConnection = nameof(DefaultConnection);

    public static void LoadServices(IServiceCollection serviceProvider, IConfiguration configuration)
    {
        serviceProvider.AddDbContext<ApplicationDbContext>(options =>
        {
            var connection = configuration.GetConnectionString(DefaultConnection);

            if (string.IsNullOrWhiteSpace(connection))
                throw new ConnectionStringException();

            options.UseSqlServer(connection, options =>
            {
                options.EnableRetryOnFailure();
            });
        });
    }

    [Serializable]
    private class ConnectionStringException : Exception { }
}
