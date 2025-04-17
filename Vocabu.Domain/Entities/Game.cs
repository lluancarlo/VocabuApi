using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class Game : ImmutableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
