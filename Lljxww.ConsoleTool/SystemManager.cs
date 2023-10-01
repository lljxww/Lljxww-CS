using System.Text.Json;
using Lljxww.ApiCaller.Models.Config;

namespace Lljxww.ConsoleTool;

public static partial class SystemManager
{
    public static void InitDefaultCallerConfig()
    {
        var apiCallerConfig = new ApiCallerConfig
        {
            Diagnosis = new DiagnosisConfig()
        };

        var jsonText = JsonSerializer.Serialize(apiCallerConfig);
        SaveCallerConfig(jsonText);
    }

    public static ActionResult SaveCallerConfigFromPath(string path)
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

            SaveCallerConfig(jsonText);
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
    public static void SaveCallerConfig(string jsonText)
    {
        var latestVersion = DbModelUtil.Instance.CallerConfigVersion;
        var currentVersion = latestVersion + 1;

        var savePath = GetCallerConfigPath(currentVersion);

        if (!Directory.Exists(Path.Combine(PathUtil.CallerConfigFileDirectory, currentVersion.ToString())))
        {
            Directory.CreateDirectory(Path.Combine(PathUtil.CallerConfigFileDirectory, currentVersion.ToString()));
        }

        if (!File.Exists(savePath))
        {
            var fs = File.Create(savePath);
            fs.Close();
            fs.Dispose();
        }

        File.WriteAllText(savePath, jsonText);

        // 更新dbModel中的版本号
        DbModelUtil.Save2File(m =>
        {
            m.CallerConfigVersion += 1;
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
    public static string GetCallerConfigPath(int version = 0)
    {
        var fileName = "caller.json";
        if (version <= 0)
        {
            return Path.Combine(PathUtil.CallerConfigFileDirectory, DbModelUtil.Instance.CallerConfigVersion.ToString(), fileName);
        }
        else
        {
            return Path.Combine(PathUtil.CallerConfigFileDirectory, version.ToString(), fileName);
        }
    }
}
