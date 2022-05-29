using System;

namespace Lljxww.Common.Utilities.LConsole;

public static class LConsole
{
    private static readonly ConsoleColor _defaultColor = ConsoleColor.Gray;

    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(message);
        Console.ForegroundColor = _defaultColor;
    }

    public static void InfoLine(string message)
    {
        Info(message);
        Console.WriteLine();
    }

    public static void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(message);
        Console.ForegroundColor = _defaultColor;
    }

    public static void WarnLine(string message)
    {
        Warn(message);
        Console.WriteLine();
    }

    public static void Fail(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(message);
        Console.ForegroundColor = _defaultColor;
    }

    public static void FailLine(string message)
    {
        Fail(message);
        Console.WriteLine();
    }
}