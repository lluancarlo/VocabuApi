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
            e.Property(p => p.Description)
                .HasMaxLength(100);
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

        InsertDefaultData(builder);
    }

    private void InsertDefaultData(ModelBuilder builder)
    {
        builder.Entity<Language>().HasData(
            new Language { Id = 1, Text = "English", Iso6391 = "en", Iso6392 = "eng" },
            new Language { Id = 2, Text = "Italian", Iso6391 = "it", Iso6392 = "ita" },
            new Language { Id = 3, Text = "Portuguese", Iso6391 = "pt", Iso6392 = "por" }
        );

        builder.Entity<PartOfSpeech>().HasData(
            new PartOfSpeech { Id = 1, Text = "Noun", Description = "A word that names a person, place, thing, or idea." },
            new PartOfSpeech { Id = 2, Text = "Verb", Description = "A word that expresses an action or a state of being." },
            new PartOfSpeech { Id = 3, Text = "Adjective", Description = "A word that describes a noun." },
            new PartOfSpeech { Id = 4, Text = "Adverb", Description = "A word that modifies a verb, adjective, or another adverb." },
            new PartOfSpeech { Id = 5, Text = "Pronoun", Description = "A word that replaces a noun." },
            new PartOfSpeech { Id = 6, Text = "Preposition", Description = "A word that shows the relationship between a noun (or pronoun) and another word." },
            new PartOfSpeech { Id = 7, Text = "Conjunction", Description = "A word that connects words, phrases, or clauses." },
            new PartOfSpeech { Id = 8, Text = "Interjection", Description = "A word or phrase that expresses emotion or exclamation." },
            new PartOfSpeech { Id = 9, Text = "Determiner", Description = "A word that introduces a noun and specifies it." },
            new PartOfSpeech { Id = 10, Text = "Article", Description = "A type of determiner: a, an, the." },
            new PartOfSpeech { Id = 11, Text = "Numeral", Description = "A word that expresses a number or order." },
            new PartOfSpeech { Id = 12, Text = "Auxiliary Verb", Description = "A helping verb used with a main verb to form a tense." },
            new PartOfSpeech { Id = 13, Text = "Modal Verb", Description = "A verb used to express ability, possibility, permission, or obligation." },
            new PartOfSpeech { Id = 14, Text = "Particle", Description = "A word that functions with a verb to form phrasal verbs." },
            new PartOfSpeech { Id = 15, Text = "Gerund", Description = "A verb form ending in -ing that functions as a noun." },
            new PartOfSpeech { Id = 16, Text = "Infinitive", Description = "The base form of a verb, often with 'to'." },
            new PartOfSpeech { Id = 17, Text = "Present Participle", Description = "A verb form ending in -ing used in continuous tenses or as adjectives." },
            new PartOfSpeech { Id = 18, Text = "Past Participle", Description = "A verb form typically ending in -ed, used in perfect tenses or as adjectives." }
    );
    }
}
