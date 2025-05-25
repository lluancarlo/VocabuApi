using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class VerbTense : ImmutableEntity
{
    public required string Text { get; set; }
    public required string Description { get; set; }
    public string? Example { get; set; }
}
