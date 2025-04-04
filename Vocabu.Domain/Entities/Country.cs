using static Vocabu.Domain.Enums;

namespace Vocabu.DAL.Entities;

public record Country
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Iso31661Alpha2 { get; set; }
    public required string Iso31661Alpha3 { get; set; }
    public required string Iso31661Numeric { get; set; }
    public required Continents Continent { get; set; }
}
