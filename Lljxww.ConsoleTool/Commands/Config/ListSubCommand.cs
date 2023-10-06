using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("list", Description = "列出已添加的配置文件")]
class ListSubCommand
{
    [Option("-l|--detail", "详细信息", CommandOptionType.SingleOrNoValue)]
    public bool WithDetail { get; }

    private void OnExecute(IConsole console)
    {
        if (DbModelUtil.Instance.CallerConfigInfos!.Count == 0)
        {
            console.WriteLine("当前未设定配置文件");
            return;
        }

        var title = "标签\t\t创建时间\t\t\t状态";
        if (WithDetail)
        {
            title += "\t路径";
        }
        console.WriteLine(title);
        foreach (var info in DbModelUtil.Instance.CallerConfigInfos!)
        {
            var content = $"{info.Tag}\t{info.CreateTime}\t\t{(info.Active ? "使用中" : "未使用")}";
            if (WithDetail)
            {
                content += info.Path;
            }
            console.WriteLine(content);
        }
    }
}
