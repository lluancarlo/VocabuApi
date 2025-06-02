namespace Vocabu.Domain.DTOs.Games;

public class PrepositionWordToImage
{
    public required PrepositionWord CorrectWord { get; set; }
    public required IEnumerable<PrepositionWord> IncorrectWords { get; set; }
}