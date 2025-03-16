namespace Vocabu.DAL.Entities;

public record Country
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public virtual required IEnumerable<User> Users { get; set; }
}
