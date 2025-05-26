using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class ApiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    #region DBSets
    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    #endregion

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(e =>
        {
            e.ToTable("Users");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();

            // ForeignKey
            e.HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Columns
            e.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(254);
            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(p => p.CountryId)
                .IsRequired();
            e.Property(p => p.Level)
                .IsRequired()
                .HasDefaultValue(1);
            e.Property(p => p.Experience)
                .IsRequired()
                .HasDefaultValue(0);
            e.Property(p => p.Gold)
                .IsRequired()
                .HasDefaultValue(0);
        });

        builder.Entity<IdentityRole<Guid>>(e =>
        {
            e.ToTable("Roles");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
        });

        builder.Entity<IdentityUserRole<Guid>>(e =>
        {
            e.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(e =>
        {
            e.ToTable("UserClaims");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
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

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
        });

        builder.Entity<Country>(e =>
        {
            e.ToTable("Countries");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Columns
            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(p => p.Iso31661Numeric)
                .IsRequired()
                .HasMaxLength(3);
            e.Property(p => p.Iso31661Alpha2)
                .IsRequired()
                .HasMaxLength(2);
            e.Property(p => p.Iso31661Alpha3)
                .IsRequired()
                .HasMaxLength(3);
            e.Property(p => p.Continent)
                .IsRequired();
        });

        builder.Entity<Language>(e =>
        {
            e.ToTable("Languages");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Columns
            e.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(20);
            e.Property(p => p.Iso6391)
                .IsRequired()
                .HasMaxLength(2);
            e.Property(p => p.Iso6392)
                .IsRequired()
                .HasMaxLength(3);
        });

        builder.Entity<Word>(e =>
        {
            e.ToTable("Words");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Columns
            e.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(20);
            e.Property(p => p.ImageUrl)
                .HasMaxLength(256);
        });

        builder.Entity<WordTypeOfSpeech>(e =>
        {
            e.ToTable("WordTypesOfSpeech");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // ForeignKey
            e.HasOne(p => p.Word)
                .WithMany()
                .HasForeignKey(p => p.WordId)
                .OnDelete(DeleteBehavior.NoAction);

            // Columns
            e.Property(p => p.PartOfSpeech)
                .IsRequired();
        });
    }
}
