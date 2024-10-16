using System;
using System.IO;
using System.Threading;

public enum LogLevel
{
    INFO,
    WARNING,
    ERROR
}

public class Logger
{
    private static Logger instance;
    private static readonly object lockObj = new object();
    private LogLevel currentLevel;
    private string logFilePath;

    private Logger() { }

    public static Logger GetInstance()
    {
        if (instance == null)
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
            }
        }
        return instance;
    }

    public void Configure(string configFilePath)
    {
        var lines = File.ReadAllLines(configFilePath);
        logFilePath = lines[0].Split('=')[1].Trim();
        currentLevel = Enum.Parse<LogLevel>(lines[1].Split('=')[1].Trim());
    }

    public void SetLogLevel(LogLevel level)
    {
        currentLevel = level;
    }

    public void Log(string message, LogLevel level)
    {
        if (level >= currentLevel)
        {
            lock (lockObj)
            {
                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"[{DateTime.Now}] [{level}] {message}");
                }
            }
        }
    }
}

public class LogReader
{
    private string logFilePath;

    public LogReader(string filePath)
    {
        logFilePath = filePath;
    }

    public void DisplayLogs(LogLevel level)
    {
        foreach (var line in File.ReadLines(logFilePath))
        {
            if (line.Contains($"[{level}]"))
            {
                Console.WriteLine(line);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var logger = Logger.GetInstance();
        logger.Configure("loggerConfig.txt");

        Thread t1 = new Thread(() => LogMessages("Thread 1", LogLevel.INFO));
        Thread t2 = new Thread(() => LogMessages("Thread 2", LogLevel.WARNING));
        Thread t3 = new Thread(() => LogMessages("Thread 3", LogLevel.ERROR));

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        var reader = new LogReader("logs.txt");
        Console.WriteLine("\nТолько ошибки:");
        reader.DisplayLogs(LogLevel.ERROR);
    }

    static void LogMessages(string threadName, LogLevel level)
    {
        var logger = Logger.GetInstance();
        for (int i = 1; i <= 5; i++)
        {
            logger.Log($"{threadName} - Сообщение {i}", level);
            Thread.Sleep(100);
        }
    }
}
