using Vocabu.Domain.Entities;
using static Vocabu.Domain.Enums;

namespace Vocabu.DAL.Entities;

public class WordMeaning : ImmutableEntity
{
    public ParthOfSpeech PartOfSpeech { get; set; }
    public required string Definition { get; set; }
    public required string Example { get; set; }
    public string? AudioUrl { get; set; }

    public virtual int WordId { get; set; }
    public Word? Word { get; set; }

    public ICollection<WordSynonym>? Synonyms { get; set; }
    public ICollection<WordAntonym>? Antonyms { get; set; }
}
