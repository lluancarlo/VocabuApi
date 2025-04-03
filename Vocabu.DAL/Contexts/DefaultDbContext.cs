using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class DefaultDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        builder.Entity<IdentityRole<Guid>>(e =>
        {
            e.ToTable("Roles");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id).IsUnique();
        });

        builder.Entity<IdentityUserRole<Guid>>(e =>
        {
            e.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(e =>
        {
            e.ToTable("UserClaims");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id).IsUnique();
        });

        builder.Entity<IdentityUserLogin<Guid>>(e =>
        {
            e.ToTable("UserLogins");
        });

        builder.Entity<IdentityUserToken<Guid>>(e =>
        {
            e.ToTable("UserTokens");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(e =>
        {
            e.ToTable("RoleClaims");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id).IsUnique();
        });

        builder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Email)
                .HasMaxLength(254);
            e.Property(p => p.Name)
                .HasMaxLength(50);
            e.HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Country>(e =>
        {
            e.ToTable("Countries");
            e.HasKey(p => p.Id);
            e.Property(p => p.Name)
                .HasMaxLength(50);
            e.Property(p => p.Iso31661Numeric)
                .HasMaxLength(3);
            e.Property(p => p.Iso31661Alpha2)
                .HasMaxLength(2);
            e.Property(p => p.Iso31661Alpha3)
                .HasMaxLength(3);
            e.Property(p => p.Continent);
        });

        builder.Entity<Game>(e =>
        {
            e.ToTable("Games");
            e.HasKey(p => p.Id);
            e.Property(p => p.Name)
                .HasMaxLength(50);
            e.Property(p => p.Description)
                .HasMaxLength(50);
        });
    }
}
