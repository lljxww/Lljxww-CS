using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool.Commands.Config;

[Command("config", Description = "配置文件操作"),
Subcommand(typeof(AddSubCommand), typeof(ListSubCommand))]
public class ConfigCommand
{
    private int OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
        return 1;
    }

    [Command("add", Description = "添加并使用Caller的配置文件")]
    private class AddSubCommand
    {
        [Required(ErrorMessage = "请指定配置文件的路径")]
        [Argument(0, Description = "配置文件的路径")]
        [FileExists]
        public string Path { get; }

        [Option("-t|--tag", "标签名", CommandOptionType.SingleValue)]
        public string Tag { get; }

        private void OnExecute(IConsole console)
        {
            var actionResult = SystemManager.SaveCallerConfigFromPath(Path, Tag);
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

    [Command("list", Description = "列出已添加的配置文件")]
    private class ListSubCommand
    {
        private void OnExecute(IConsole console)
        {
            if (DbModelUtil.Instance.CallerConfigInfos!.Count == 0)
            {
                console.WriteLine("当前未设定配置文件");
                return;
            }

            console.WriteLine("已添加的配置文件：");
            console.WriteLine($"标签\t版本\t创建时间\t\t\t状态");
            foreach (var info in DbModelUtil.Instance.CallerConfigInfos!)
            {
                console.WriteLine($"{info.Tag}\t{info.Version}\t{info.CreateTime}\t\t{info.Active}");
            }
        }
    }
}
