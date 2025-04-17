using Vocabu.Domain.Entities;
using static Vocabu.Domain.Enums;

namespace Vocabu.DAL.Entities;

public class Word : ImmutableEntity
{
    public required string Text { get; set; }
    public Languages Language { get; set; }

    public virtual ICollection<WordMeaning>? WordMeanings { get; set; }
}
