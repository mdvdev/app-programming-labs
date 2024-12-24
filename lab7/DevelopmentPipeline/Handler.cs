namespace lab7;

using System;
using System.Threading.Tasks;
using System.Threading.Channels;

public class Handler : IHandler
{
    private readonly ChannelReader<ITaskItem> _inputChannel;
    private readonly ChannelWriter<ITaskItem>? _outputChannel;
    private readonly int _processingTime;
    private readonly string _name;
    private readonly IEventLogger _logger;
    private readonly IMetrics _metrics;
    private readonly IStageManager _stageManager;
    private readonly Random _random = new();
    private int _outputQueueCount = 0;

    public Handler(ChannelReader<ITaskItem> inputChannel, ChannelWriter<ITaskItem>? outputChannel,
        int processingTime, string name, IEventLogger logger, IMetrics metrics, IStageManager stageManager)
    {
        _inputChannel = inputChannel ?? throw new ArgumentNullException(nameof(inputChannel));
        _outputChannel = outputChannel;
        _processingTime = processingTime;
        _name = name;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        _stageManager = stageManager ?? throw new ArgumentNullException(nameof(stageManager));
    }

    public async Task StartAsync()
    {
        await foreach (var task in _inputChannel.ReadAllAsync())
        {
            _metrics.TrackQueueLength(_name, _inputChannel.Count);

            await _logger.LogAsync($"{_name} started processing {task}");

            int delay = _processingTime + _random.Next(-_processingTime / 5, _processingTime / 5);
            await Task.Delay(delay);

            await _logger.LogAsync($"{_name} finished processing {task}");

            if (_outputChannel != null)
            {
                Interlocked.Increment(ref _outputQueueCount);
                _metrics.TrackQueueLength(_name, _outputQueueCount);

                await _outputChannel.WriteAsync(task);

                Interlocked.Decrement(ref _outputQueueCount);
            }
        }

        await _stageManager.ReportCompletionAsync(_name, _logger);
    }
}