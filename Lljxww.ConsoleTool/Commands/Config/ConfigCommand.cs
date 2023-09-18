using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("config", Description = "配置文件操作"), Subcommand(typeof(Add))]
public class ConfigCommand
{
    private int OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
        return 1;
    }

    [Command("add", Description = "指定当前使用的配置文件")]
    private class Add
    {
        [Required(ErrorMessage = "请指定配置文件的路径")]
        [Argument(0, Description = "配置文件的路径")]
        [FileExists]
        public string Path { get; }

        private void OnExecute(IConsole console)
        {
            var actionResult = SystemManager.SetCallerConfigPath(Path);
            if (actionResult.Success)
            {
                console.Success("设置成功，后续将使用的配置文件为：");
                console.Success(Path);
            }
            else
            {
                console.Error(actionResult.Message);
            }
        }
    }
}
