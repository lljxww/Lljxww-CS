using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Lljxww.ApiCaller.Models.Config;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

[Command("edit", Description = "编辑配置文件")]
class EditSubCommand
{
    [Required(ErrorMessage = "请输入要编辑的配置文件的标签")]
    [Argument(0, Description = "要编辑的配置文件的标签")]
    public string Tag { get; }

    private int OnExecute(IConsole console)
    {
        var actionResult = SystemManager.GetCallerConfig(Tag);
        if (!actionResult.Success)
        {
            console.Error(actionResult.Message);
            return -1;
        }

        var (filePath, fileContent) = actionResult.Content;
        try
        {
            var apiCallerConfig = JsonSerializer.Deserialize<ApiCallerConfig>(fileContent);

            if (apiCallerConfig == null)
            {
                console.WriteLine("当前配置文件为空");
                return 1;
            }

            // ServiceItem
            var serviceItemResult = ConfigEditor.NodeSelecter(console, apiCallerConfig.ServiceItems);
            if (!serviceItemResult.Success)
            {
                return -1;
            }

            // ApiItem
            var apiItemResult = ConfigEditor.NodeSelecter(console, serviceItemResult.Content.ApiItems);
            if (!apiItemResult.Success)
            {
                return -1;
            }

            var current = apiItemResult.Content;

            // 处理节点编辑
            console.Write(JsonSerializer.Serialize(current, WriteIndentedJsonSerializerOptions.GetInstance));

            return 1;
        }
        catch (Exception ex)
        {
            console.Error($"配置解析失败: {ex.Message}");
            return -1;
        }
    }
}