using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class Score : MutableEntity
{
    public int Points { get; set; }

    #region Join Entities
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public int GameId { get; set; }
    public virtual Game? Game { get; set; }
    #endregion

    public Score(int points, Guid userId, int gameId)
    {
        Id = Guid.NewGuid();
        Points = points;
        UserId = userId;
        GameId = gameId;
    }
}
