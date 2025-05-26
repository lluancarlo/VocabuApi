using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class GeneratorDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    #region DBSets
    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    #endregion

    public GeneratorDbContext(DbContextOptions<GeneratorDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        InsertDefaultData(builder);
    }

    private void InsertDefaultData(ModelBuilder builder)
    {
        builder.Entity<Language>().HasData(
            new Language { Id = 1, Text = "English", Iso6391 = "en", Iso6392 = "eng" },
            new Language { Id = 2, Text = "Italian", Iso6391 = "it", Iso6392 = "ita" },
            new Language { Id = 3, Text = "Portuguese", Iso6391 = "pt", Iso6392 = "por" }
        );
    }
}
