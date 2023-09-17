using Lljxww.ConsoleTool;
using McMaster.Extensions.CommandLineUtils;

[Command(Name = "caller-util", Description = "基于Lljxww.Caller的的网络请求命令行工具"), Subcommand(typeof(ConfigCommand))]
public class Program
{
    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    private int OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
        return 1;
    }
}
