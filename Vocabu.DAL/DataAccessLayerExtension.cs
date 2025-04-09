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

    public static void LoadServices<TContext>(IServiceCollection serviceProvider, IConfiguration configuration) 
        where TContext : DbContext
    {
        serviceProvider.AddDbContext<TContext>(options =>
        {
            var connection = configuration.GetConnectionString(DefaultConnection);
            if (string.IsNullOrWhiteSpace(connection))
                throw new ConnectionStringException();

            options.UseNpgsql(connection, options =>
            {
                options.MigrationsAssembly("Vocabu.DAL");
                options.EnableRetryOnFailure();
            });
        });

        serviceProvider.AddScoped(typeof(IRepository<>), typeof(ApiGenericRepository<>));
    }

    private class ConnectionStringException : Exception { }
}
