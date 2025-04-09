using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class ApiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    #region DBSets
    public DbSet<Country> Countries { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Score> Score { get; set; }
    public DbSet<ScoreTransaction> ScoreTransaction { get; set; }
    public DbSet<JobLog> JobLogs { get; set; }
    #endregion

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
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
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
        });

        builder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(254);
            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(p => p.CountryId)
                .IsRequired();
            e.HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Country>(e =>
        {
            e.ToTable("Countries");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();
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

        builder.Entity<Game>(e =>
        {
            e.ToTable("Games");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(50);
        });

        builder.Entity<Score>(e =>
        {
            e.ToTable("Scores");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Points)
                .IsRequired()
                .HasDefaultValue(0);
            e.Property(p => p.UserId)
                .IsRequired();
            e.Property(p => p.GameId)
                .IsRequired();
            e.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);
            e.HasOne(p => p.Game)
                .WithMany()
                .HasForeignKey(p => p.GameId);
        });

        builder.Entity<ScoreTransaction>(e =>
        {
            e.ToTable("ScoreTransactions");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Points)
                .IsRequired();
            e.Property(p => p.ExecutedAt)
                .IsRequired();
            e.HasOne(p => p.Score)
                .WithMany()
                .HasForeignKey(p => p.ScoreId);
        });

        builder.Entity<JobLog>(e =>
        {
            e.ToTable("JobLogs");
            e.HasKey(j => j.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(j => j.JobName)
                .HasMaxLength(100)
                .IsRequired();
            e.Property(j => j.LastRun)
                .IsRequired();
            e.Property(j => j.LastRunSuccess)
                .IsRequired();
            e.Property(j => j.Result)
                .HasMaxLength(1000);
        });
    }
}
