using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class ApiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private const string DataSchema = "Data";
    private const string ReferenceSchema = "Ref";
    private const string ConfigSchema = "Config";
    private const string AuthSchema = "Auth";

    #region DBSets
    public DbSet<Country> Countries { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<PartOfSpeech> PartsOfSpeech { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<VerbMode> VerbalModes { get; set; }
    public DbSet<Conjugation> Conjugations { get; set; }

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
            
        #region ReferenceSchema
        builder.Entity<Country>(e =>
        {
            e.ToTable("Countries", ReferenceSchema);

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

        builder.Entity<Game>(e =>
        {
            e.ToTable("Games", ReferenceSchema);

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
            e.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(50);
        });

        builder.Entity<Language>(e =>
        {
            e.ToTable("Languages", ReferenceSchema);

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

        builder.Entity<PartOfSpeech>(e =>
        {
            e.ToTable("PartsOfSpeech", ReferenceSchema);

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
        });

        builder.Entity<Word>(e =>
        {
            e.ToTable("Words", ReferenceSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // ForeignKey
            e.HasOne(h => h.Language)
                .WithMany()
                .HasForeignKey(h => h.LanguageId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // Columns
            e.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(20);
        });

        builder.Entity<WordPartOfSpeech>(e =>
        {
            e.ToTable("WordsPartsOfSpeech", ReferenceSchema);

            // PrimaryKey
            e.HasKey(p => new { p.WordId, p.PartOfSpeechId });

            // ForeignKey
            e.HasOne(h => h.Word)
                .WithMany()
                .HasForeignKey(h => h.WordId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            e.HasOne(h => h.PartOfSpeech)
                .WithMany()
                .HasForeignKey(h => h.PartOfSpeechId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        });

        builder.Entity<VerbMode>(e =>
        {
            e.ToTable("VerbalModes", ReferenceSchema);

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
            e.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(100);
            e.Property(p => p.Example)
                .HasMaxLength(100);
        });

        builder.Entity<Conjugation>(e =>
        {
            e.ToTable("Conjugations", ReferenceSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // ForeignKey
            e.HasOne(h => h.Word)
                .WithMany()
                .HasForeignKey(h => h.WordId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            e.HasOne(h => h.VerbalMode)
                .WithMany()
                .HasForeignKey(h => h.VerbalModeId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // Columns
            e.Property(p => p.FirstSingular)
                .IsRequired()
                .HasMaxLength(30);
            e.Property(p => p.SecondSingular)
                .IsRequired()
                .HasMaxLength(30);
            e.Property(p => p.ThirdSingular)
                .IsRequired()
                .HasMaxLength(30);
            e.Property(p => p.FirstPlural)
                .IsRequired()
                .HasMaxLength(30);
            e.Property(p => p.SecondPlural)
                .IsRequired()
                .HasMaxLength(30);
            e.Property(p => p.ThirdPlural)
                .IsRequired()
                .HasMaxLength(30);
            e.Property(p => p.NoPersonal)
                .HasMaxLength(30);
        });
        #endregion

        #region DataSchema
        builder.Entity<Score>(e =>
        {
            e.ToTable("Scores", DataSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();

            // ForeignKey
            e.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            e.HasOne(p => p.Game)
                .WithMany()
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // Columns
            e.Property(p => p.Points)
                .IsRequired()
                .HasDefaultValue(0);
        });

        builder.Entity<ScoreTransaction>(e =>
        {
            e.ToTable("ScoreTransactions", DataSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();

            // ForeignKey
            e.HasOne(p => p.Score)
                .WithMany()
                .HasForeignKey(p => p.ScoreId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // Columns
            e.Property(p => p.Points)
                .IsRequired();
            e.Property(p => p.ExecutedAt)
                .IsRequired();
        });
        #endregion

        #region ConfigSchema
        builder.Entity<JobLog>(e =>
        {
            e.ToTable("JobLogs", ConfigSchema);

            // PrimaryKey
            e.HasKey(j => j.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();

            // Columns
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
        #endregion

        #region AuthSchema
        builder.Entity<IdentityRole<Guid>>(e =>
        {
            e.ToTable("Roles", AuthSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
        });

        builder.Entity<IdentityUserRole<Guid>>(e =>
        {
            e.ToTable("UserRoles", AuthSchema);
        });

        builder.Entity<IdentityUserClaim<Guid>>(e =>
        {
            e.ToTable("UserClaims", AuthSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
        });

        builder.Entity<IdentityUserLogin<Guid>>(e =>
        {
            e.ToTable("UserLogins", AuthSchema);
        });

        builder.Entity<IdentityUserToken<Guid>>(e =>
        {
            e.ToTable("UserTokens", AuthSchema);
        });

        builder.Entity<IdentityRoleClaim<Guid>>(e =>
        {
            e.ToTable("RoleClaims", AuthSchema);

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
        });

        builder.Entity<User>(e =>
        {
            e.ToTable("Users", AuthSchema);

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
        });
        #endregion
    }
}
