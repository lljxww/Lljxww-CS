using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("cleanup", Description = "删除未使用的配置文件")]
class CleanupSubCommand
{
    private void OnExecute(IConsole console)
    {
        if (DbModelUtil.Instance.CallerConfigInfos!.Count != 0)
        {
            DbModelUtil.UpdateDbModel(instance =>
            {
                var info = instance.CallerConfigInfos.Single(i => i.Active);
                instance.CallerConfigInfos = new List<CallerConfigInfo> {
                        info
                };
                return instance;
            });
        }

        SystemManager.Cleanup();

        console.Success("清理完成");
    }
}
