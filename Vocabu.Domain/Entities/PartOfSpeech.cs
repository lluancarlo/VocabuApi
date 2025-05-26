using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class PartOfSpeech : ImmutableEntity
{
    public required string Text { get; set; }
    public string? Description { get; set; }
}
