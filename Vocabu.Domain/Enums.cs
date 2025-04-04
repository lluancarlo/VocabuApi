namespace Vocabu.Domain;

public static class Enums
{
    public enum Continents
    {
        Africa = 0,
        Antarctica = 1,
        Asia = 2,
        Europe = 3,
        NorthAmerica = 4,
        Oceania = 5,
        SouthAmerica = 6
    }

    public static T GetEnumByName<T>(string name) => (T)Enum.Parse(typeof(T), name.Trim());
}
