using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Vocabu.DAL.Contexts;

/// <summary>
/// EF tools use IDesignTimeDbContextFactory only at design-time 
/// (when running add-migration or update-database).
/// </summary>
class DefaultDbContextFactory : IDesignTimeDbContextFactory<DefaultDbContext>
{
    public DefaultDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json") // path might depend on your folder structure
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DefaultDbContext>();
        var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new DefaultDbContext(optionsBuilder.Options);
    }
}
