using Vocabu.DAL.Entities;

namespace Vocabu.Domain.DTOs.Games;

public class PrepositionWord
{
    public required string Word { get; set; }
    public required string WordImage { get; set; }
    public bool isCorrect { get; set; }

    public static PrepositionWord FromWordEntity(Word entity) =>
        new PrepositionWord 
        {
            Word = entity.Text,
            WordImage = entity.ImageUrl!
        };
}