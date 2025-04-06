namespace Vocabu.DAL.Entities;

public class Score
{
    public Guid Id { get; set; }
    public int Points { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public Guid GameId { get; set; }
    public virtual Game? Game { get; set; }

    public Score(int points, Guid userId, Guid gameId)
    {
        Id = Guid.NewGuid();
        Points = points;
        UserId = userId;
        GameId = gameId;
    }
}
