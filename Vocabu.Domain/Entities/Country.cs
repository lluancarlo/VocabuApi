namespace Vocabu.DAL.Entities;

public record Country
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Continent { get; set; }

    public virtual IEnumerable<User>? Users { get; set; }
}
