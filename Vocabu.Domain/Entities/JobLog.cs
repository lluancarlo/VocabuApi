namespace Vocabu.DAL.Entities;

public class JobLog
{
    public Guid Id { get; set; }
    public string JobName { get; set; }
    public DateTime LastRun { get; set; }
    public bool LastRunSuccess { get; set; }
    public string? Result { get; set; }

    public JobLog(string jobName, DateTime lastRun, bool lastRunSuccess, string? result = null)
    {
        Id = Guid.NewGuid();
        JobName = jobName;
        LastRun = lastRun;
        LastRunSuccess = lastRunSuccess;
        Result = result;
    }
}
