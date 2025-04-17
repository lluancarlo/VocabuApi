using Vocabu.Domain.Entities;
using static Vocabu.Domain.Enums;

namespace Vocabu.DAL.Entities;

public class Country : ImmutableEntity
{
    public string Name { get; set; }
    public string Iso31661Alpha2 { get; set; }
    public string Iso31661Alpha3 { get; set; }
    public string Iso31661Numeric { get; set; }
    public Continents Continent { get; set; }
}
