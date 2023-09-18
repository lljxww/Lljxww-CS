using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Lljxww.ApiCaller;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("request", Description = "发送请求")]
public class RequestCommand
{
    [Required(ErrorMessage = "请指定要调用的目标")]
    [Argument(0, Description = "调用的目标")]
    public string Target { get; }

    [Option("-p|--param", Description = "请求参数，使用Json格式")]
    public string Param { get; }

    private async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
        var caller = new Caller(SystemManager.GetCallerConfigPath());

        if (!caller.HasTarget(Target))
        {
            console.Error($"找不到指定的目标: {Target}");
            return -1;
        }

        try
        {
            var result = await caller.InvokeAsync(Target, string.IsNullOrWhiteSpace(Param) ? null : JsonSerializer.Deserialize<object>(Param));

            console.Success(JsonTextUtil.JsonPrettify(result.RawStr));
            return 1;
        }
        catch (Exception ex)
        {
            console.Error($"请求失败: {ex.Message}");
            return -1;
        }
    }
}
