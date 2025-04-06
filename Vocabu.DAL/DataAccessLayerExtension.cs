using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vocabu.DAL.Contexts;
using Vocabu.DAL.Repositories;
using Vocabu.Domain.Interfaces;

namespace Vocabu.DAL;

public static class DataAccessLayerExtension
{
    public const string DefaultConnection = nameof(DefaultConnection);

    public static void LoadServices(IServiceCollection serviceProvider, IConfiguration configuration)
    {
        serviceProvider.AddDbContext<DefaultDbContext>(options =>
        {
            var connection = configuration.GetConnectionString(DefaultConnection);
            if (string.IsNullOrWhiteSpace(connection))
                throw new ConnectionStringException();

            options.UseSqlServer(connection, options =>
            {
                options.MigrationsAssembly("Vocabu.DAL");
                options.EnableRetryOnFailure();
            });
        });

        serviceProvider.AddScoped(typeof(IRepository<>), typeof(DefaultRepository<>));
    }

    private class ConnectionStringException : Exception { }
}
