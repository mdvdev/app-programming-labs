namespace lab7;

public interface IStageManager
{
    public Task ReportCompletionAsync(string name, IEventLogger logger);
}