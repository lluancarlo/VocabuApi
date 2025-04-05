namespace Vocabu.DAL.Entities;

public class Score
{
    public Guid Id { get; set; }
    public required int Points { get; set; }

    public required Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public required Guid GameId { get; set; }
    public virtual Game? Game { get; set; }
}
