namespace Vocabu.DAL.Entities;

public class WordSynonym
{
    public virtual int WordId { get; set; }
    public Word? Word { get; set; }

    public virtual int MeaningId { get; set; }
    public WordMeaning? WordMeaning { get; set; }
}
    