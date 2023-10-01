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

            SaveCallerConfig(jsonText, tag);
            return new ActionResult
            {
                Success = true
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
    /// 存储配置文件到系统中
    /// </summary>
    /// <param name="jsonText"></param>
    /// <param name="tag"></param>
    private static void SaveCallerConfig(string jsonText, string? tag = null)
    {
        var latestVersion = DbModelUtil.Instance.CallerConfigVersion;
        var currentVersion = latestVersion + 1;

        var saveDirectory = Path.Combine(PathUtil.CallerConfigFileDirectory, currentVersion.ToString());
        var savePath = GetCallerConfigPathToSave();

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
        DbModelUtil.Save2File(m =>
        {
            m.CallerConfigVersion += 1;
            m.CallerConfigInfos ??= new List<CallerConfigInfo>();

            if (m.CallerConfigInfos?.Count != 0)
            {
                var activedInfo = m.CallerConfigInfos!.SingleOrDefault(i => i.Active);
                if (activedInfo != default)
                {
                    m.CallerConfigInfos!.Single(i => i.Tag == activedInfo.Tag).Active = false;
                }
            }

            var callerConfigInfo = new CallerConfigInfo
            {
                Directory = saveDirectory,
                Path = savePath,
                Active = true,
                Version = m.CallerConfigVersion
            };

            if (!string.IsNullOrWhiteSpace(tag))
            {
                callerConfigInfo.Tag = tag!;
            }

            m.CallerConfigInfos!.Add(callerConfigInfo);

            m.CallerConfigInfos = m.CallerConfigInfos
                .Where(i => i.Version > m.CallerConfigVersion - m.MaxCallerConfigVersion)
                .ToList();

            return m;
        });

        // 删除超过版本期限的数据
        var directoriesToBeDeleted = Directory.GetDirectories(PathUtil.CallerConfigFileDirectory);
        foreach (var directoryName in directoriesToBeDeleted)
        {
            if (int.Parse(directoryName.Split(Path.DirectorySeparatorChar)[^1]) < (latestVersion - DbModelUtil.Instance.MaxCallerConfigVersion))
            {
                Directory.Delete(directoryName);
            }
        }
    }

    /// <summary>
    /// 当前使用的配置文件路径
    /// </summary>
    internal static string? GetCallerConfigPath(int version = 0)
    {
        var infos = DbModelUtil.Instance.CallerConfigInfos;
        if (infos.Count == 0)
        {
            return null;
        }

        return infos.Single(i => i.Active).Path;
    }

    /// <summary>
    /// 获取存储caller.json的路径
    /// </summary>
    /// <returns></returns>
    internal static string GetCallerConfigPathToSave()
    {
        int version = DbModelUtil.Instance.CallerConfigVersion + 1;
        return Path.Combine(PathUtil.CallerConfigFileDirectory, version.ToString(), PathUtil.CALLER_CONFIG_FILE_NAME);
    }
}
