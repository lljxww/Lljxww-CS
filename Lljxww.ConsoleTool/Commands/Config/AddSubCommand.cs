using Lljxww.ConsoleTool.Utils;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace Lljxww.ConsoleTool.Commands.Config;

[Command("add", Description = "添加并使用Caller的配置文件")]
internal class AddSubCommand
{
    [Required(ErrorMessage = "请指定配置文件的路径")]
    [Argument(0, Description = "配置文件的路径")]
    [FileExists]
    public string Path { get; }

    [Option("-t|--tag", "标签名", CommandOptionType.SingleValue)]
    public string Tag { get; }

    private void OnExecute(IConsole console)
    {
        ActionResult actionResult = SystemManager.SaveCallerConfigFromPath(Path, Tag);
        if (actionResult.Success)
        {
            console.Success($"设置成功, tag: {actionResult.Message}");
        }
        else
        {
            console.Error(actionResult.Message);
        }
    }
}
