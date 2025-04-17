using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Entities;

namespace Vocabu.DAL.Contexts;

public class GeneratorDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    #region DBSets
    public DbSet<Country> Countries { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<WordMeaning> WordMeanings { get; set; }
    public DbSet<WordSynonym> WordSynonyms { get; set; }
    public DbSet<WordAntonym> WordAntonyms { get; set; }
    #endregion

    public GeneratorDbContext(DbContextOptions<GeneratorDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

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

        builder.Entity<Word>(e =>
        {
            e.ToTable("Words");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            e.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(20);
            e.Property(p => p.Language)
                .IsRequired();
        });

        builder.Entity<WordMeaning>(e =>
        {
            e.ToTable("WordMeanings");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Id)
                .IsUnique();
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            e.Property(p => p.PartOfSpeech)
                .IsRequired()
                .HasMaxLength(50);
            e.Property(p => p.Definition)
                .IsRequired();
            e.Property(p => p.Example)
                .HasMaxLength(200);
            e.Property(p => p.AudioUrl)
                .HasMaxLength(200);
            e.HasOne(p => p.Word)
                .WithMany(p => p.WordMeanings)
                .HasForeignKey(p => p.WordId);
        });

        builder.Entity<WordSynonym>(e =>
        {
            e.ToTable("WordSynonyms");
            e.HasKey(p => new { p.WordId, p.MeaningId });

            e.HasOne(p => p.WordMeaning)
                .WithMany(p => p.Synonyms)
                .HasForeignKey(p => p.MeaningId);

            e.HasOne(p => p.Word)
                .WithMany()
                .HasForeignKey(p => p.WordId);
        });

        builder.Entity<WordAntonym>(e =>
        {
            e.ToTable("WordAntonyms");
            e.HasKey(p => new { p.MeaningId, p.WordId });

            e.HasOne(p => p.WordMeaning)
                .WithMany(p => p.Antonyms)
                .HasForeignKey(p => p.MeaningId);

            e.HasOne(p => p.Word)
                .WithMany()
                .HasForeignKey(p => p.WordId);
        });
    }
}
