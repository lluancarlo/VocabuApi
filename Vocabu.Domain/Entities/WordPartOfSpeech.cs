namespace Vocabu.DAL.Entities;

public class WordPartOfSpeech
{
    public int WordId { get; set; }
    public Word? Word { get; set; }

    public int PartOfSpeechId { get; set; }
    public PartOfSpeech? PartOfSpeech { get; set; }
}
