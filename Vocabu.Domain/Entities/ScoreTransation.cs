using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class ScoreTransaction : MutableEntity
{
    public int Points { get; set; }
    public DateTime ExecutedAt { get; set; }

    #region Join Entities
    public Guid ScoreId { get; set; }
    public virtual Score? Score { get; set; }
    #endregion

    public ScoreTransaction(int points, DateTime executedAt, Guid scoreId)
    {
        Id = Guid.NewGuid();
        Points = points;
        ExecutedAt = executedAt;
        ScoreId = scoreId;
    }
}
