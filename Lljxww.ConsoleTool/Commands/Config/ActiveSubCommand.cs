using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("active", Description = "激活指定的配置文件")]
class ActiveSubCommand
{
    [Required(ErrorMessage = "请输入配置文件的标签")]
    [Argument(0, Description = "要使用的配置文件的标签")]
    public string Tag { get; }

    private void OnExecute(IConsole console)
    {
        if (DbModelUtil.Instance.CallerConfigInfos!.Count == 0)
        {
            console.WriteLine("当前未设定配置文件");
            return;
        }

        if (!DbModelUtil.Instance.CallerConfigInfos.Any(i => i.Tag == Tag))
        {
            console.Error($"找不到指定标签({Tag})的配置文件");
        }

        DbModelUtil.UpdateDbModel(instance =>
        {
            instance.CallerConfigInfos.Single(i => i.Active).Active = false;
            instance.CallerConfigInfos.Single(i => i.Tag == Tag).Active = true;

            return instance;
        });

        console.Success($"配置文件 {Tag} 已激活");
    }
}
