namespace lab7;

using System;
using System.IO;

public class EventLogger : IEventLogger
{
    private readonly string _logFilePath;
    private readonly bool _appendToFile;
    private StreamWriter _logWriter;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public EventLogger(string logFilePath, bool appendToFile = false)
    {
        _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
        _appendToFile = appendToFile;

        _logWriter = new StreamWriter(_logFilePath, _appendToFile, System.Text.Encoding.UTF8)
        {
            AutoFlush = true
        };
    }

    public async Task LogAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Warning: Attempted to log an empty or null message.");
            return;
        }

        var logMessage = $"{DateTime.Now:HH:mm:ss} - {message}";
        Console.WriteLine(logMessage);

        await _semaphore.WaitAsync();
        try
        {
            await _logWriter.WriteLineAsync(logMessage);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _logWriter?.Dispose();
        _semaphore?.Dispose();
    }
}
