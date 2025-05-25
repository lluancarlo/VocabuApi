using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class Word : ImmutableEntity
{
    public int LanguageId { get; set; }
    public Language? Language { get; set; }

    public required string Text { get; set; }
}
