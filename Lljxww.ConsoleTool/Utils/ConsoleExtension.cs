using System.Text.Json;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

public static class ConsoleExtension
{
    public static void PrintLn(this IConsole console, string message, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null)
    {
        if (foregroundColor != null)
        {
            console.ForegroundColor = foregroundColor.Value;
        }

        if (backgroundColor != null)
        {
            console.BackgroundColor = backgroundColor.Value;
        }

        console.WriteLine(message);
        console.ResetColor();
    }

    public static void Error(this IConsole console, string errMessage)
    {
        PrintLn(console, errMessage, ConsoleColor.Red);
    }

    public static void Success(this IConsole console, string successMessage)
    {
        PrintLn(console, successMessage, ConsoleColor.Green);
    }

    public static void IndentPrint(this IConsole console, object? value)
    {
        if (value == null)
        {
            PrintLn(console, "[null]");
            return;
        }

        PrintLn(console, JsonSerializer.Serialize(value, WriteIndentedJsonSerializerOptions.GetInstance));
    }
}
