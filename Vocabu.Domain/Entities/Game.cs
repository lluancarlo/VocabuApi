namespace Vocabu.DAL.Entities;

public class Game
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
