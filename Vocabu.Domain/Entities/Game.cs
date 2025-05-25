using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class Game : ImmutableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}
