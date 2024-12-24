using System.Threading.Channels;

namespace lab7;

public class Analyst : Handler
{
    public Analyst(ChannelReader<ITaskItem> inputChannel, ChannelWriter<ITaskItem>? outputChannel, int processingTime,
        string name, IEventLogger logger, IMetrics metrics, IStageManager stageManager)
        : base(inputChannel, outputChannel, processingTime, name, logger, metrics, stageManager)  
    {
    }
}