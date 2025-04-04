using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vocabu.DAL.Contexts;

namespace Generator.Common;

public static class DbContext
{
    private static IServiceScope? _currentScope;

    private const string DefaultConnection = nameof(DefaultConnection);
    private class ConnectionStringException : Exception { }

    private static IHost ConnectToDatabase() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DefaultDbContext>(options =>
                {
                    var connection = context.Configuration.GetConnectionString(DefaultConnection);
                    if (string.IsNullOrWhiteSpace(connection))
                        throw new ConnectionStringException();

                    options.UseSqlServer(connection);
                });
            })
            .Build();

    private static IServiceScope GetScope()
    {
        if (_currentScope == null)
            _currentScope = ConnectToDatabase().Services.CreateScope();

        return _currentScope;
    }

    public static DefaultDbContext GetDataBaseService() => GetScope().ServiceProvider.GetRequiredService<DefaultDbContext>();
}
