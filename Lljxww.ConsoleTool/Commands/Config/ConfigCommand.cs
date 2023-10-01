using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool.Commands.Config;

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
            var actionResult = SystemManager.SaveCallerConfigFromPath(Path);
            if (actionResult.Success)
            {
                console.Success("设置成功");
                if (!DbModelUtil.Instance.SilentMode)
                {
                    console.WriteLine($"当前版本: {DbModelUtil.Instance.CallerConfigVersion}");
                }
            }
            else
            {
                console.Error(actionResult.Message);
            }
        }
    }
}
