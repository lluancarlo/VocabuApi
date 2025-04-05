namespace Vocabu.DAL.Entities;

public class JobLog
{
    public Guid Id { get; set; }
    public required string JobName { get; set; }
    public required DateTime LastRun { get; set; }
    public required bool LastRunSuccess { get; set; }
    public string? Result { get; set; }
}
