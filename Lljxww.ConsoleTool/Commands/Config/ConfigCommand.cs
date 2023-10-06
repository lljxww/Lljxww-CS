using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool.Commands.Config;

[Command("config", Description = "配置文件操作"),
    Subcommand(typeof(AddSubCommand),
        typeof(ListSubCommand),
        typeof(ActiveSubCommand),
        typeof(CleanupSubCommand),
        typeof(RemoveSubCommand),
        typeof(EditSubCommand))]
public class ConfigCommand
{
    private int OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
        return 1;
    }
}
