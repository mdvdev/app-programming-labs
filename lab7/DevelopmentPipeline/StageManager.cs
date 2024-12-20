namespace lab7;

using System.Threading.Channels;
using System.Threading.Tasks;

public class StageManager : IStageManager
{
    private int _activeHandlers;
    private readonly ChannelWriter<ITaskItem>? _outputChannel;

    public StageManager(ChannelWriter<ITaskItem>? outputChannel, int handlerCount)
    {
        _outputChannel = outputChannel;
        _activeHandlers = handlerCount;
    }

    public async Task ReportCompletionAsync(string name, IEventLogger logger)
    {
        if (Interlocked.Decrement(ref _activeHandlers) == 0)
        {
            await CompleteStageAsync(name, logger);
        }
    }

    private async Task CompleteStageAsync(string name, IEventLogger logger)
    {
        if (_outputChannel != null)
        {
            try
            {
                await logger.LogAsync($"{name} has finished all tasks.");
                _outputChannel.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completing stage: {ex.Message}");
            }
        }
    }
}