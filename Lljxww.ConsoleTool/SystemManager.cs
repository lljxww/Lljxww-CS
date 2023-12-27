using Lljxww.ApiCaller.Models.Config;
using Lljxww.ConsoleTool.Utils;
using System.Text.Json;

namespace Lljxww.ConsoleTool;

internal static partial class SystemManager
{
    /// <summary>
    /// 保存并使用Caller的配置文件
    /// </summary>
    /// <param name="path">用户指定的配置文件路径</param>
    /// <param name="tag">标签名</param>
    /// <returns></returns>
    internal static ActionResult SaveCallerConfigFromPath(string path, string? tag = null)
    {
        if (!File.Exists(path))
        {
            return new ActionResult
            {
                Success = false,
                Message = $"找不到文件: {path}"
            };
        }

        string jsonText = File.ReadAllText(path);
        try
        {
            _ = JsonSerializer.Deserialize<ApiCallerConfig>(jsonText)
                ?? throw new JsonException();

            tag ??= TimestampUtil.GetCurrent();
            SaveCallerConfig(jsonText, tag);
            return new ActionResult
            {
                Success = true,
                Message = tag
            };
        }
        catch (JsonException)
        {
            return new ActionResult
            {
                Success = false,
                Message = $"文件格式错误: {path}"
            };
        }
    }

    /// <summary>
    /// 当前使用的配置文件路径
    /// </summary>
    internal static string? GetCallerConfigPath()
    {
        IList<CallerConfigInfo> infos = DbModelUtil.Instance.CallerConfigInfos;
        return infos.Count == 0 ? null : infos.Single(i => i.Active).Path;
    }

    /// <summary>
    /// 使用tag查询指定的配置文件路径和内容
    /// </summary>
    /// <param name="tag">标签名</param>
    /// <returns></returns>
    internal static ActionResult<(string, string)> GetCallerConfig(string tag)
    {
        string targetDirectoryPath = Path.Combine(PathUtil.CallerConfigFileDirectory, tag);
        string targetFilePath = Path.Combine(targetDirectoryPath, PathUtil.CALLER_CONFIG_FILE_NAME);
        if (!Directory.Exists(targetDirectoryPath) || !File.Exists(targetFilePath))
        {
            return new ActionResult<(string, string)>
            {
                Success = false,
                Message = $"不存在标签为`{tag}`的配置文件"
            };
        }

        (string, string) resultTuple = new(targetFilePath, File.ReadAllText(targetFilePath));
        return new ActionResult<(string, string)>
        {
            Success = true,
            Content = resultTuple
        };
    }

    /// <summary>
    /// 获取存储caller.json的路径
    /// </summary>
    /// <returns></returns>
    internal static string GetCallerConfigPathToSave(string tag)
    {
        return Path.Combine(PathUtil.CallerConfigFileDirectory, tag, PathUtil.CALLER_CONFIG_FILE_NAME);
    }

    /// <summary>
    /// 删除已不在配置文件中的Caller配置文件路径
    /// </summary>config
    internal static void Cleanup()
    {
        List<string>? pathsToBeCleanup = Directory.GetDirectories(PathUtil.CallerConfigFileDirectory)?.ToList();
        if (pathsToBeCleanup?.Count == 0)
        {
            return;
        }

        pathsToBeCleanup = pathsToBeCleanup!.Except(DbModelUtil.Instance.CallerConfigInfos
            .Select(i => i.Directory))?.ToList();

        if (pathsToBeCleanup?.Count == 0)
        {
            return;
        }

        foreach (string? path in pathsToBeCleanup!)
        {
            Directory.Delete(path, true);
        }
    }

    /// <summary>
    /// 保存配置文件(更新)
    /// </summary>
    /// <param name="config"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static ActionResult SaveUpdatedCallerConfig(ApiCallerConfig config, string path)
    {
        string jsonText = JsonSerializer.Serialize(config);
        if (!File.Exists(path))
        {
            return new ActionResult
            {
                Success = false,
                Message = $"指定的文件不存在: {path}"
            };
        }

        // 清空文件
        File.WriteAllText(path, string.Empty);

        File.WriteAllText(path, jsonText);

        return new ActionResult
        {
            Success = true
        };
    }

    /// <summary>
    /// 存储配置文件到系统中
    /// </summary>
    /// <param name="jsonText"></param>
    /// <param name="tag"></param>
    private static void SaveCallerConfig(string jsonText, string tag)
    {
        string saveDirectory = Path.Combine(PathUtil.CallerConfigFileDirectory, tag);
        string savePath = GetCallerConfigPathToSave(tag);

        if (!Directory.Exists(saveDirectory))
        {
            _ = Directory.CreateDirectory(saveDirectory);
        }

        if (!File.Exists(savePath))
        {
            FileStream fs = File.Create(savePath);
            fs.Close();
            fs.Dispose();
        }

        File.WriteAllText(savePath, jsonText);

        // 更新dbModel
        DbModelUtil.UpdateDbModel(m =>
        {
            m.CallerConfigInfos ??= new List<CallerConfigInfo>();

            if (m.CallerConfigInfos.Count != 0)
            {
                CallerConfigInfo? activedInfo = m.CallerConfigInfos!.SingleOrDefault(i => i.Active);
                if (activedInfo != default)
                {
                    m.CallerConfigInfos!.Single(i => i.Tag == activedInfo.Tag).Active = false;
                }
            }

            CallerConfigInfo callerConfigInfo = new()
            {
                Tag = tag,
                Directory = saveDirectory,
                Path = savePath,
                Active = true
            };

            m.CallerConfigInfos.Add(callerConfigInfo);

            return m;
        });
    }
}
