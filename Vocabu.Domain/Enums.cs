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

    public enum Languages
    {
        English = 0,
        Italian = 1,
        Portuguese = 2,
    }

    public enum ParthOfSpeech
    {
        Nouns = 0,
        Pronouns = 1,
        Verbs = 2,
        Adjectives = 3,
        Adverbs = 4,
        Prepositions = 5,
        Conjunctions = 6,
        Interjections = 7,
    }

    public static T GetEnumByName<T>(string name) => (T)Enum.Parse(typeof(T), name.Trim());
}
