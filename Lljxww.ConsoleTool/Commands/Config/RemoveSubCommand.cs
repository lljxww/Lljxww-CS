using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("remove", Description = "删除指定的配置文件")]
class RemoveSubCommand
{
    [Required(ErrorMessage = "请输入要删除的配置文件的标签")]
    [Argument(0, Description = "要删除的配置文件的标签")]
    public string Tag { get; }

    private int OnExecute(IConsole console)
    {
        if (!DbModelUtil.Instance.CallerConfigInfos.Any(i => string.Equals(Tag, i.Tag)))
        {
            console.Error($"找不到配置文件: {Tag}");
            return -1;
        }

        DbModelUtil.UpdateDbModel(instance =>
        {
            instance.CallerConfigInfos
                .Remove(instance.CallerConfigInfos.Single(i => string.Equals(Tag, i.Tag)));
            return instance;
        });

        SystemManager.Cleanup();

        console.Success("清理完成");
        return 1;
    }
}
