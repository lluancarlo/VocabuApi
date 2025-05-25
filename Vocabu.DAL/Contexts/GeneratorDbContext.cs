using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class GeneratorDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    #region DBSets
    public DbSet<Country> Countries { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<PartOfSpeech> PartsOfSpeech { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<VerbMode> VerbalModes { get; set; }
    public DbSet<Conjugation> Conjugations { get; set; }
    #endregion

    public GeneratorDbContext(DbContextOptions<GeneratorDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Ref");

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

        builder.Entity<Game>(e =>
        {
            e.ToTable("Games");

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

        builder.Entity<PartOfSpeech>(e =>
        {
            e.ToTable("PartsOfSpeech");

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
            e.ToTable("Words");

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
            e.ToTable("WordsPartsOfSpeech");

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
            e.ToTable("VerbModes");

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

        builder.Entity<VerbTense>(e =>
        {
            e.ToTable("VerbTenses");

            // PrimaryKey
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Columns
            e.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(100);
            e.Property(p => p.Example)
                .HasMaxLength(100);
        });

        builder.Entity<Conjugation>(e =>
        {
            e.ToTable("Conjugations");

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
    }
}
