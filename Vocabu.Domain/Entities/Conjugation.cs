using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class Conjugation : ImmutableEntity
{
    public int WordId { get; set; }
    public Word? Word { get; set; }

    public int VerbalModeId { get; set; }
    public VerbMode? VerbalMode { get; set; }

    public required string FirstSingular { get; set; }
    public required string SecondSingular { get; set; }
    public required string ThirdSingular { get; set; }
    public required string FirstPlural { get; set; }
    public required string SecondPlural { get; set; }
    public required string ThirdPlural { get; set; }
    public string? NoPersonal { get; set; }
}
