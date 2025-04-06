namespace Vocabu.DAL.Entities;

public class ScoreTransaction
{
    public Guid Id { get; set; }
    public int Points { get; set; }
    public DateTime ExecutedAt { get; set; }

    public Guid ScoreId { get; set; }
    public virtual Score? Score { get; set; }

    public ScoreTransaction(int points, DateTime executedAt, Guid scoreId)
    {
        Id = Guid.NewGuid();
        Points = points;
        ExecutedAt = executedAt;
        ScoreId = scoreId;
    }
}
