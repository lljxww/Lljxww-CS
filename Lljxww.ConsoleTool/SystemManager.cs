using System.Text.Json;
using Lljxww.ApiCaller.Models.Config;

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

        var jsonText = File.ReadAllText(path);
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
        var infos = DbModelUtil.Instance.CallerConfigInfos;
        if (infos.Count == 0)
        {
            return null;
        }

        return infos.Single(i => i.Active).Path;
    }

    /// <summary>
    /// 使用tag查询指定的配置文件路径和内容
    /// </summary>
    /// <param name="tag">标签名</param>
    /// <returns></returns>
    internal static ActionResult<(string, string)> GetCallerConfig(string tag)
    {
        var targetDirectoryPath = Path.Combine(PathUtil.CallerConfigFileDirectory, tag);
        var targetFilePath = Path.Combine(targetDirectoryPath, PathUtil.CALLER_CONFIG_FILE_NAME);
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
        var pathsToBeCleanup = Directory.GetDirectories(PathUtil.CallerConfigFileDirectory)?.ToList();
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

        foreach (var path in pathsToBeCleanup!)
        {
            Directory.Delete(path, true);
        }
    }

    /// <summary>
    /// 存储配置文件到系统中
    /// </summary>
    /// <param name="jsonText"></param>
    /// <param name="tag"></param>
    private static void SaveCallerConfig(string jsonText, string tag)
    {
        var saveDirectory = Path.Combine(PathUtil.CallerConfigFileDirectory, tag);
        var savePath = GetCallerConfigPathToSave(tag);

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        if (!File.Exists(savePath))
        {
            var fs = File.Create(savePath);
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
                var activedInfo = m.CallerConfigInfos!.SingleOrDefault(i => i.Active);
                if (activedInfo != default)
                {
                    m.CallerConfigInfos!.Single(i => i.Tag == activedInfo.Tag).Active = false;
                }
            }

            var callerConfigInfo = new CallerConfigInfo
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
