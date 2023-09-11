using Lljxww.ApiCaller;
using Lljxww.ConsoleTool;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.ConfigureCaller();

var app = new CommandLineApplication();

app.HelpOption();

var subject = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);
subject.DefaultValue = "world";

var repeat = app.Option<int>("-n|--count <N>", "Repeat", CommandOptionType.SingleValue);
repeat.DefaultValue = 1;

app.OnExecute(() =>
{
    for (var i = 0; i < repeat.ParsedValue; i++)
    {
        Console.WriteLine($"Hello {subject.Value()}!");
    }
});

// 配置文件操作
app.Command("config", cfgCmd =>
{
    cfgCmd.OnExecute(() =>
    {
        Console.WriteLine("请指定一个子命令");
        cfgCmd.ShowHelp();
        return 1;
    });

    // 添加配置文件，将替换已有的配置
    cfgCmd.Command("add", addCfgCmd =>
    {
        addCfgCmd.Description = "添加用于此工具的请求配置文件";
        var path = addCfgCmd.Argument("path", "配置文件的路径").IsRequired();

        addCfgCmd.OnExecute(() =>
        {
            if (string.IsNullOrWhiteSpace(path.Value))
            {
                Console.WriteLine("请输入有效的配置文件路径");
            }

            if (!File.Exists(path.Value))
            {
                Console.WriteLine($"给定的配置文件不存在: {path.Value}");
            }

            //TODO 检查配置文件有效性

            Console.WriteLine("配置文件已添加：");
            Console.WriteLine($"{path.Value}");
        });
    });
});

app.Command("request", reqCmd =>
{
    var actionResult = CheckConfig();
    if (!actionResult.Success)
    {
        reqCmd.ShowHelp();
        return;
    }

    var key = reqCmd.Argument("key", "请求的目标，使用\".\"分隔服务名与终结点名，如\"github.getUserInfo\"")
        .IsRequired();
});

return app.Execute(args);

/// <sammary>
/// 检查必要的配置文件
/// </sammary>
static ActionResult<string> CheckConfig()
{
    string configPath = LocalDBUtil.ConfigPath;

    if (string.IsNullOrWhiteSpace(configPath))
    {
        return new ActionResult<string>
        {
            Success = false,
            Message = "请添加请求配置文件路径"
        };
    }

    return new ActionResult<string>
    {
        Success = true,
        Content = configPath
    };
}