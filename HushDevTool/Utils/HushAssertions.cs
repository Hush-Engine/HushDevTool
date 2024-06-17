using System;
namespace HushDevTool.Utils;

public static class HushAssertions
{
    public static void AssertTrue(bool condition, string message = "")
    {
        if (!condition)
        {
            throw new HushException(message);
        }
    }

    public static void AssertNotNull<T>(T? obj, string message = "") where T : class
    {
        if (obj == null)
        {
            throw new HushException(message);
        }
    }
}

public class HushException : Exception
{
    public HushException(string? message) : base($"Assertion error!\n{message}")
    {

    }
}

