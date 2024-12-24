using System.Threading.Channels;

namespace lab7;

public class Tester : Handler
{
    public Tester(ChannelReader<ITaskItem> inputChannel, ChannelWriter<ITaskItem>? outputChannel, int processingTime,
        string name, IEventLogger logger, IMetrics metrics, IStageManager stageManager)
        : base(inputChannel, outputChannel, processingTime, name, logger, metrics, stageManager)
    {
    }
}