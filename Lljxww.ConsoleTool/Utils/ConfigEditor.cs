using Lljxww.ApiCaller;
using Lljxww.ApiCaller.Models.Config;
using McMaster.Extensions.CommandLineUtils;

namespace Lljxww.ConsoleTool;

internal class ConfigEditor
{
    private static readonly int RETRY_TIMES = 3;

    internal static ActionResult<T> NodeSelecter<T>(IConsole console, IList<T>? nodes) where T : ICallerConfigNode
    {
        if (nodes == null || nodes.Count == 0)
        {
            console.WriteLine("当前配置中的节点配置为空");
        }

        int i = 0;
        var dic = new Dictionary<int, T>();
        foreach (var node in nodes!)
        {
            dic.Add(++i, node);
            console.WriteLine($"{i} {node.Remark()}");
        }

        T? current = default;

        int times = 0;
        while (times < RETRY_TIMES)
        {
            int index = Prompt.GetInt("请输入要编辑的节点序号：");

            if (index <= 0 || !dic.ContainsKey(index))
            {
                console.Error($"请输入正确的索引");
                times++;
                continue;
            }
            else
            {
                current = dic[index];
                if (current == null)
                {
                    console.WriteLine("当前配置中的节点配置为空");
                    return new ActionResult<T>
                    {
                        Success = false
                    };
                }
                else if (current is ServiceItem serviceItem)
                {
                    if (serviceItem.ApiItems?.Count == 0)
                    {
                        console.WriteLine("当前配置中的节点配置为空");
                        return new ActionResult<T>
                        {
                            Success = false
                        };
                    }
                }

                return new ActionResult<T>
                {
                    Success = true,
                    Content = current!
                };
            }
        }

        if (times == RETRY_TIMES)
        {
            return new ActionResult<T>
            {
                Success = false
            };
        }

        return new ActionResult<T>
        {
            Success = true,
            Content = current!
        };
    }
}
