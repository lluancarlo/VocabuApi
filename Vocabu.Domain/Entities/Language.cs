using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class Language : ImmutableEntity
{
    public required string Text { get; set; }
    public required string Iso6391 { get; set; }
    public required string Iso6392 { get; set; }
}
