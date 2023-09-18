using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

public static class ConsoleExtension
{
    public static void Error(this IConsole console, string errMessage)
    {
        console.ForegroundColor = ConsoleColor.Red;
        console.WriteLine(errMessage);
        console.ResetColor();
    }

    public static void Success(this IConsole console, string successMessage)
    {
        console.ForegroundColor = ConsoleColor.Green;
        console.WriteLine(successMessage);
        console.ResetColor();
    }
}
