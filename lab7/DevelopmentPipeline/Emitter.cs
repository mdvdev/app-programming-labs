namespace lab7;

using System;
using System.Threading.Channels;
using System.Threading.Tasks;

public class Emitter : IEmitter
{
    private readonly ChannelWriter<ITaskItem> _queueWriter;
    private readonly ICollection<ITaskItem> _tasks;
    private readonly int _baseFrequency;
    private readonly IEventLogger _logger;
    private readonly Random _random = new();

    public Emitter(ChannelWriter<ITaskItem> queueWriter, int baseFrequency, IEventLogger logger,
        ICollection<ITaskItem> tasks)
    {
        _queueWriter = queueWriter ?? throw new ArgumentNullException(nameof(queueWriter));
        _baseFrequency = baseFrequency;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tasks = tasks ?? throw new ArgumentNullException(nameof(tasks));
    }

    public async Task StartAsync(int taskCount)
    {
        for (var i = 1; i <= taskCount; i++)
        {
            ITaskItem task = new TaskItem(i);
            _tasks.Add(task);
            await _queueWriter.WriteAsync(task);
            
            await _logger.LogAsync($"Emitter generated {task}");

            var randomDelay = _baseFrequency + _random.Next(-_baseFrequency / 10, _baseFrequency / 10);
            await Task.Delay(randomDelay);
        }

        _queueWriter.Complete(); 
    }
}
