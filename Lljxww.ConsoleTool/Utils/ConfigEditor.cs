using System.Reflection;
using Lljxww.ApiCaller.Models.Config;
using Lljxww.ConsoleTool.Models;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
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
    internal static ActionResult<T> NodeSelecter<T>(IConsole console, IList<T>? nodes)
        where T : ICallerConfigNode
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

            if (index <= 0 || !dic.TryGetValue(index, out current))
            {
                console.Error($"请输入正确的索引");
                times++;
                continue;
            }
            else
            {
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

        PropertyInfo[] properties = typeof(T).GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            object? currentValue = properties[i].GetValue(node);
            _ = console.WriteLine($"节点名: {properties[i].Name}, 当前值: {currentValue ?? "(null)"}");

            string message = "请输入新的值";
            object? result = default;
            switch (properties[i].PropertyType.Name)
            {
                case nameof(TypeCode.String):
                    {
                        result = Prompt.GetString(message, currentValue?.ToString());
                        break;
                    }
                case nameof(TypeCode.Int32):
                    {
                        if (int.TryParse(currentValue?.ToString(), out int defaultValue))
                        {
                            result = Prompt.GetInt(message, defaultValue);
                        }
                        else
                        {
                            result = Prompt.GetInt(message);
                        }
                        break;
                    }
                case nameof(TypeCode.Boolean):
                    {
                        _ = bool.TryParse(currentValue?.ToString(), out bool defaultValue);
                        result = Prompt.GetYesNo(message, defaultValue);
                        break;
                    }
                default:
                    {
                        // IList
                        break;
                    }
            }

            if (result == default)
            {
                continue;
            }

            // 设定值
            properties[i].SetValue(node, result);
        }

        console.IndentPrint(node);

        return new ActionResult<T>
        {
            Success = true,
            Content = node
        };
    }

    /// <summary>
    /// 从给定的配置文件中，找到VID为指定值的节点
    /// </summary>
    /// <param name="config"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static NodeEditModel? GetNode(ApiCallerConfig config, Guid id)
    {
        if (config == null || config.ServiceItems?.Count == 0)
        {
            return default;
        }

        var serviceItemResult = config.ServiceItems!.SingleOrDefault(s => s.ID == id);
        if (serviceItemResult != null)
        {
            return new NodeEditModel
            {
                Node = serviceItemResult,
                Index = config.ServiceItems!.IndexOf(serviceItemResult),
                Type = typeof(ServiceItem)
            };
        }

        foreach (var service in config.ServiceItems!)
        {
            if (service.ApiItems?.Count == 0)
            {
                continue;
            }

            var apiItemResult = service.ApiItems!.SingleOrDefault(a => a.ID == id);
            if (apiItemResult != null)
            {
                return new NodeEditModel
                {
                    Node = apiItemResult,
                    ParentIndex = config.ServiceItems.IndexOf(service),
                    Index = service.ApiItems!.IndexOf(apiItemResult),
                    Type = typeof(ApiItem)
                };
            }
        }

        return default;
    }

    /// <summary>
    /// 更新CallerCongig
    /// </summary>
    /// <param name="config"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static ApiCallerConfig? UpdateApiCallerConfig(ApiCallerConfig config, ICallerConfigNode node)
    {
        var nodeEditModel = GetNode(config, node.GetID());
        if (nodeEditModel == default)
        {
            return config;
        }

        if (nodeEditModel.Type == typeof(ServiceItem))
        {
            config.ServiceItems[nodeEditModel.Index] = (ServiceItem)node;
        }
        else
        {
            config.ServiceItems[nodeEditModel.ParentIndex].ApiItems[nodeEditModel.Index] = (ApiItem)node;
        }

        return config;
    }
}
