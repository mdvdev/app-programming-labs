namespace lab7;

public interface IEmitter
{
    Task StartAsync(int taskCount);
}