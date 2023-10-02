using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool.Commands.Config;

[Command("config", Description = "配置文件操作"),
    Subcommand(typeof(AddSubCommand),
        typeof(ListSubCommand),
        typeof(ActiveSubCommand),
        typeof(CleanupSubCommand),
        typeof(RemoveSubCommand))]
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
                console.Success($"设置成功, tag: {Tag}");
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
        [Option("-l|--detail", "详细信息", CommandOptionType.SingleOrNoValue)]
        public bool WithDetail { get; }

        private void OnExecute(IConsole console)
        {
            if (DbModelUtil.Instance.CallerConfigInfos!.Count == 0)
            {
                console.WriteLine("当前未设定配置文件");
                return;
            }

            console.WriteLine("已添加的配置文件：");
            var title = "标签\t创建时间\t\t\t状态";
            if (WithDetail)
            {
                title += "\t路径";
            }
            console.WriteLine(title);
            foreach (var info in DbModelUtil.Instance.CallerConfigInfos!)
            {
                var content = $"{info.Tag}\t{info.CreateTime}\t\t{info.Active}";
                if (WithDetail)
                {
                    content += info.Path;
                }
                console.WriteLine(content);
            }
        }
    }

    [Command("active", Description = "激活指定的配置文件")]
    private class ActiveSubCommand
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

    [Command("remove", Description = "删除指定的配置文件")]
    private class RemoveSubCommand
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

    [Command("cleanup", Description = "删除未使用的配置文件")]
    private class CleanupSubCommand
    {
        private void OnExecute(IConsole console)
        {
            if (DbModelUtil.Instance.CallerConfigInfos!.Count != 0)
            {
                DbModelUtil.UpdateDbModel(instance =>
                {
                    var info = instance.CallerConfigInfos.Single(i => i.Active);
                    instance.CallerConfigInfos = new List<CallerConfigInfo>{
                        info
                    };
                    return instance;
                });
            }

            SystemManager.Cleanup();

            console.Success("清理完成");
        }
    }
}
