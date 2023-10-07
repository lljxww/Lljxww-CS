using Lljxww.ApiCaller.Models.Config;
using McMaster.Extensions.CommandLineUtils;
using Result = (System.Type, object);

namespace Lljxww.ConsoleTool.Utils;

internal class ConfigEditor
{
    private static readonly int RETRY_TIMES = 3;

    /// <summary>
    /// 选择节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="console"></param>
    /// <param name="nodes"></param>
    /// <returns></returns>
    internal static ActionResult<T> NodeSelecter<T>(IConsole console, IList<T>? nodes) where T : ICallerConfigNode
    {
        if (nodes == null || nodes.Count == 0)
        {
            _ = console.WriteLine("当前配置中的节点配置为空");
        }

        int i = 0;
        Dictionary<int, T> dic = [];
        foreach (T node in nodes!)
        {
            dic.Add(++i, node);
            _ = console.WriteLine($"{i} {node.Remark()}");
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
                    _ = console.WriteLine("当前配置中的节点配置为空");
                    return new ActionResult<T>
                    {
                        Success = false
                    };
                }
                else if (current is ServiceItem serviceItem)
                {
                    if (serviceItem.ApiItems?.Count == 0)
                    {
                        _ = console.WriteLine("当前配置中的节点配置为空");
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

        return times == RETRY_TIMES
            ? new ActionResult<T>
            {
                Success = false
            }
            : new ActionResult<T>
            {
                Success = true,
                Content = current!
            };
    }

    /// <summary>
    /// 编辑节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static ActionResult<T> Edit<T>(IConsole console, T node) where T : ICallerConfigNode
    {
        if (node == null)
        {
            return new ActionResult<T>
            {
                Success = false,
                Message = "当前节点为空"
            };
        }

        System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (System.Reflection.PropertyInfo prop in properties)
        {
            object? currentValue = prop.GetValue(node);
            _ = console.WriteLine($"节点名: {prop.Name}, 当前值: {currentValue}");

            string message = "请输入新的值";
            Result result = default;
            switch (Type.GetTypeCode(prop.DeclaringType))
            {
                case TypeCode.String:
                    {
                        string? value = Prompt.GetString(message, currentValue?.ToString());
                        result = (typeof(string), value)!;
                        break;
                    }
                case TypeCode.Int32:
                    {
                        if (int.TryParse(currentValue?.ToString(), out int defaultValue))
                        {
                            int value = Prompt.GetInt(message, defaultValue);
                            result = (typeof(int), value)!;
                        }
                        else
                        {
                            int value = Prompt.GetInt(message);
                            result = (typeof(int), value)!;
                        }
                        break;
                    }
                case TypeCode.Boolean:
                    {
                        _ = bool.TryParse(currentValue?.ToString(), out bool defaultValue);
                        bool value = Prompt.GetYesNo(message, defaultValue);
                        result = (typeof(bool), value);
                        break;
                    }
                default:
                    {
                        // IList
                        break;
                    }
            }

            if (result.Item1 == null)
            {
                continue;
            }

            // 设定值
            prop.SetValue(node, Convert.ChangeType(result.Item2, result.Item1!));
        }

        console.IndentPrint(node);

        return new ActionResult<T>
        {
            Success = true,
            Content = node
        };
    }
}
