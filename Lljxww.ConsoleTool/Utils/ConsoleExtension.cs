using McMaster.Extensions.CommandLineUtils;
using System.Text.Json;

namespace Lljxww.ConsoleTool.Utils;

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

        _ = console.WriteLine(message);
        console.ResetColor();
    }

    public static void Error(this IConsole console, string errMessage)
    {
        console.PrintLn(errMessage, ConsoleColor.Red);
    }

    public static void Success(this IConsole console, string successMessage)
    {
        console.PrintLn(successMessage, ConsoleColor.Green);
    }

    public static void IndentPrint(this IConsole console, object? value)
    {
        if (value == null)
        {
            console.PrintLn("[null]");
            return;
        }

        console.PrintLn(JsonSerializer.Serialize(value, WriteIndentedJsonSerializerOptions.GetInstance));
    }
}
