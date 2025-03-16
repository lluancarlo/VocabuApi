namespace Vocabu.DAL.Entities;

public record User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }

    public Guid CountryId { get; set; }
    public virtual required Country Country { get; set; }
}