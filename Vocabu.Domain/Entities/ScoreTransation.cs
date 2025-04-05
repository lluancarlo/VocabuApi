namespace Vocabu.DAL.Entities;

public class ScoreTransaction
{
    public Guid Id { get; set; }
    public required int Points { get; set; }
    public required DateTime ExecutedAt { get; set; }

    public required Guid ScoreId { get; set; }
    public virtual Score? Score { get; set; }
}
