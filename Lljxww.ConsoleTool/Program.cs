using System.Text.Json;
using Lljxww.ApiCaller;
using Lljxww.ConsoleTool;
using McMaster.Extensions.CommandLineUtils;

public class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandLineApplication();

        app.HelpOption();

        Caller _caller = default;

        app.OnExecute(() =>
        {
            var actionResult = CheckConfig();
            if (!actionResult.Success)
            {
                Console.WriteLine(actionResult.Message);
                app.ShowHelp();
                return;
            }
            else
            {
                _caller = new Caller(actionResult.Content);
            }
        });

        // // 配置文件操作
        // app.Command("config", cfgCmd =>
        // {
        //     cfgCmd.OnExecute(() =>
        //     {
        //         Console.WriteLine("请指定一个子命令");
        //         cfgCmd.ShowHelp();
        //         return 1;
        //     });

        //     // 添加配置文件，将替换已有的配置
        //     cfgCmd.Command("add", addCfgCmd =>
        //     {
        //         addCfgCmd.Description = "添加用于此工具的请求配置文件";
        //         var path = addCfgCmd.Option("-p|--path", "配置文件的路径", CommandOptionType.SingleValue)
        //             .IsRequired()
        //             .Accepts(v => v.ExistingFile());

        //         addCfgCmd.OnExecute(() =>
        //         {
        //             //TODO 检查配置文件有效性

        //             var localDb = LocalDBUtil.GetFileInstance();
        //             localDb.CallerConfigPath = path.Value();
        //             LocalDBUtil.Save(localDb);

        //             _caller = new Caller(path.Value());

        //             Console.WriteLine("配置文件已添加：");
        //             Console.WriteLine($"{path.Value}");
        //         });
        //     });
        // });

        app.Command("request", reqCmd =>
        {
            var key = reqCmd.Option("-k|--key", "请求的目标，使用\".\"分隔服务名与终结点名，如\"github.getUserInfo\"", CommandOptionType.SingleValue)
                .IsRequired()
                .Value();
            var param = reqCmd.Option("-p|--param", "请求的参数，json格式。可选", CommandOptionType.SingleValue)?.Value();

            reqCmd.OnExecuteAsync(async _ =>
            {
                reqCmd.Description = "发起指定的请求";

                var actionResult = CheckConfig();
                if (!actionResult.Success)
                {
                    reqCmd.ShowHelp();
                    return;
                }

                object paramObj = null;
                if (!string.IsNullOrWhiteSpace(param))
                {
                    paramObj = JsonSerializer.Deserialize<object>(param);
                }

                var result = await _caller.InvokeAsync(key.Trim(), param);
                Console.WriteLine(result.RawStr);
            });
        });

        return app.Execute(args);
    }

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
}
