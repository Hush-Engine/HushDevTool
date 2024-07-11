using System;
namespace HushDevTool.Utils;

public static class Logger
{
    private static ConsoleColor s_initialColor = Console.ForegroundColor;

    static Logger()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
    }

    public static void Debug(string message)
    {
#if DEBUG
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(message);
        Console.ForegroundColor = s_initialColor;
#endif
    }

    public static void Info(string message)
    {
        Console.WriteLine(message);
    }

    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ForegroundColor = s_initialColor;
    }

    public static void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠️ - {message}");
        Console.ForegroundColor = s_initialColor;
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[E] - {message}");
        Console.ForegroundColor = s_initialColor;
    }

    public static void Error(Exception err)
    {
        Type exceptionType = err.GetType();
        if (exceptionType == typeof(Cocona.CoconaException) || exceptionType == typeof(HushException))
        {
            //We probably sent this ourselves, so, no need for stack trace
            Error(err.Message);
            return;
        }
        //Otherwise, it's an unexpected error, send a stack trace
        Error($"{err.Message}\n{err.StackTrace}");
    }

}

