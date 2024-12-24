using Core;

namespace Presentation;

class Program
{
    static async Task Main(string[] args)
    {
        ICommandProcessor processor = new ConsoleCommandProcessor();
        await processor.RunAsync();
    }
}