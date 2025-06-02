using Vocabu.Domain.Entities;
using static Vocabu.Domain.Enums;

namespace Vocabu.DAL.Entities;

public class WordPreposition : ImmutableEntity
{
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }

    public PrepositionType Type { get; set; }
    public required string Example { get; set; }
}
