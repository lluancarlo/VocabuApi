using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class DefaultDbContext : DbContext
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        builder.Entity<User>(e =>
        {
            e.ToTable(nameof(Users));

            e.HasKey(p => p.Id);

            e.HasIndex(p => p.Id).IsUnique();

            e.Property(p => p.Email)
                .HasMaxLength(256);

            e.Property(p => p.Password)
                .HasMaxLength(256);

            e.Property(p => p.Name)
                .HasMaxLength(50);

            e.HasOne(p => p.Country)
                .WithMany(t => t.Users)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Country>(e =>
        {
            e.ToTable(nameof(Countries));
            e.HasKey(p => p.Id);

            e.Property(p => p.Name)
                .HasMaxLength(50);
        });
    }
}
