namespace lab7;

using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    private const string LogPath = "log.txt";
    private const string ConfigPath = "config.json";

    static async Task Main()
    {
        IConfiguration? config = LoadConfiguration();
        if (config == null) return;

        int emitterFrequency = config.GetValue<int>("EmitterFrequency");
        int handlerProcessingTime = config.GetValue<int>("HandlerProcessingTime");
        int taskCount = config.GetValue<int>("TaskCount");
        int analystCount = config.GetValue<int>("AnalystCount");
        int developerCount = config.GetValue<int>("DeveloperCount");
        int testerCount = config.GetValue<int>("TesterCount");
        int managerCount = config.GetValue<int>("ManagerCount");

        IEventLogger logger = new EventLogger(LogPath, appendToFile: false);

        var queueAnalytics = Channel.CreateUnbounded<ITaskItem>();
        var queueDevelopment = Channel.CreateUnbounded<ITaskItem>();
        var queueTesting = Channel.CreateUnbounded<ITaskItem>();
        var queueManager = Channel.CreateUnbounded<ITaskItem>();

        IMetrics metrics = new Metrics();
        
        // List of generated tasks for metrics calculation later
        List<ITaskItem> generatedTasks = [];

        IEmitter emitter = new Emitter(queueAnalytics.Writer, emitterFrequency, logger, generatedTasks);

        var pipelineTasks = new List<Task>
        {
            emitter.StartAsync(taskCount),
            StartHandlers(queueAnalytics.Reader, queueDevelopment.Writer, handlerProcessingTime, "Analyst", analystCount, metrics, logger),
            StartHandlers(queueDevelopment.Reader, queueTesting.Writer, handlerProcessingTime, "Developer", developerCount, metrics, logger),
            StartHandlers(queueTesting.Reader, queueManager.Writer, handlerProcessingTime, "Tester", testerCount, metrics, logger),
            StartHandlers(queueManager.Reader, null, handlerProcessingTime, "Manager", managerCount, metrics, logger)
        };

        try
        {
            await Task.WhenAll(pipelineTasks);
        }
        catch (Exception ex)
        {
            await logger.LogAsync($"An error occurred: {ex.Message}");
        }

        await logger.LogAsync($"Average task idle time: {metrics.CalculateAverageIdleTime(generatedTasks):F2} ms");
        await logger.LogAsync($"Max queue lengths: {metrics.GetMaxQueueLengthsReport()}");
        await logger.LogAsync($"Pipeline completed. Logs written to {LogPath}");
    }

    static IConfiguration? LoadConfiguration()
    {
        try
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigPath, optional: false, reloadOnChange: true)
                .Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load configuration: {ex.Message}");
            return null;
        }
    }

    static Task StartHandlers(ChannelReader<ITaskItem> inputChannel, ChannelWriter<ITaskItem>? outputChannel,
        int processingTime, string stageName, int handlerCount, IMetrics metrics, IEventLogger logger)
    {
        var tasks = new List<Task>();
        IStageManager stageManager = new StageManager(outputChannel, handlerCount);

        for (int i = 0; i < handlerCount; i++)
        {
            IHandler handler = stageName switch
            {
                "Analyst" => new Analyst(inputChannel, outputChannel, processingTime, $"{stageName}-{i + 1}", logger,
                    metrics, stageManager),
                "Developer" => new Developer(inputChannel, outputChannel, processingTime, $"{stageName}-{i + 1}",
                    logger, metrics, stageManager),
                "Tester" => new Tester(inputChannel, outputChannel, processingTime, $"{stageName}-{i + 1}", logger,
                    metrics, stageManager),
                _ => new Manager(inputChannel, outputChannel, processingTime, $"{stageName}-{i + 1}", logger, metrics,
                    stageManager)
            };

            tasks.Add(handler.StartAsync());
        }

        return Task.WhenAll(tasks);
    }
}