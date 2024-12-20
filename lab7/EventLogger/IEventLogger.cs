namespace lab7;

public interface IEventLogger : IDisposable
{
    public Task LogAsync(string message);
}