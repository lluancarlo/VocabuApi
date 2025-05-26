using Vocabu.Domain.Entities;
using static Vocabu.Domain.Enums;

namespace Vocabu.DAL.Entities;

public class WordTypeOfSpeech : ImmutableEntity
{
    public int WordId { get; set; }
    public Word? Word { get; set; }

    public PartsOfSpeech PartOfSpeech { get; set; }
}
