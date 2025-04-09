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

    public Country(string name, string iso31661Alpha2, string iso31661Alpha3, 
        string iso31661Numeric, Continents continent)
    {
        Name = name;
        Iso31661Alpha2 = iso31661Alpha2;
        Iso31661Alpha3 = iso31661Alpha3;
        Iso31661Numeric = iso31661Numeric;
        Continent = continent;
    }
}
