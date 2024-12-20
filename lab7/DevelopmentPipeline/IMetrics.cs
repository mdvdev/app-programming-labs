namespace lab7;

public interface IMetrics
{
    public void TrackQueueLength(string handlerName, int currentLength);
    
    public string GetMaxQueueLengthsReport();
    
    public double CalculateAverageIdleTime(IEnumerable<ITaskItem> tasks);
}