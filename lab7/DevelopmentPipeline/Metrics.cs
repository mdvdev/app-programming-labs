namespace lab7;

using System.Collections.Concurrent;
using System.Linq;

public class Metrics : IMetrics
{
    private readonly ConcurrentBag<double> _processingTimes = new();
    private readonly ConcurrentDictionary<string, int> _maxQueueLengths = new();

    public void TrackQueueLength(string handlerName, int currentLength)
    {
        _maxQueueLengths.AddOrUpdate(handlerName, currentLength,
            (_, currentMax) => Math.Max(currentMax, currentLength));
    }

    public string GetMaxQueueLengthsReport()
    {
        return string.Join(", ", _maxQueueLengths.Select(kv => $"{kv.Key}: {kv.Value}"));
    }

    public double CalculateAverageIdleTime(IEnumerable<ITaskItem> tasks)
    {
        var currentTime = DateTime.Now;
        var taskItems = tasks as ITaskItem[] ?? tasks.ToArray();
        var sum = taskItems.Sum(task => (currentTime - task.CreatedAt).TotalMilliseconds);

        return sum / taskItems.Length;
    }
}

