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

    public enum PartsOfSpeech
    {
        Noun = 0,
        Verb = 1,
        Adjective = 2,
        Adverb = 3,
        Pronoun = 4,
        Preposition = 5,
        Conjunction = 6,
        Interjection = 7,
        Determiner = 8,
        Article = 9,
        Numeral = 10,
        Auxiliary = 11,
        Modal = 12
    }

    public static T GetEnumByName<T>(string name) => (T)Enum.Parse(typeof(T), name.Trim());
}
