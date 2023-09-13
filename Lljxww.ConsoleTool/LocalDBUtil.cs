using System.Text;
using System.Text.Json;

namespace Lljxww.ConsoleTool;

public class LocalDBUtil
{
    /// <summary>
    /// 查询指定自定义信息的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string Get(string key)
    {
        var dbModel = GetFileInstance();

        if (dbModel.Settings.TryGetValue(key, out string value))
        {
            return value;
        }

        return default;
    }

    /// <summary>
    /// 设置一个自定义信息
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void Set(string key, string value)
    {
        var dbModel = GetFileInstance();

        if (dbModel.Settings.ContainsKey(key))
        {
            dbModel.Settings[key] = value;
        }
        else
        {
            dbModel.Settings.Add(key, value);
        }

        Save(dbModel);
    }

    /// <summary>
    /// 配置文件的路径
    /// </summary>
    public static string ConfigPath => GetFileInstance().CallerConfigPath;

    /// <summary>
    /// 程序数据文件的路名
    /// </summary>
    private static string FileDirectory => Path.Combine(Environment.CurrentDirectory, "config");

    /// <summary>
    /// 配置文件的目录路径
    /// </summary>
    private static string FilePath => Path.Combine(FileDirectory, "setting.json");

    /// <summary>
    /// 初始化本地文件
    /// </summary>
    /// <returns></returns>
    public static DbModel GetFileInstance()
    {
        if (!Directory.Exists(FileDirectory))
        {
            Directory.CreateDirectory(FileDirectory);
        }

        DbModel dbModel = new();

        if (File.Exists(FilePath))
        {
            var jsonString = File.ReadAllText(FilePath, Encoding.UTF8);
            try
            {
                dbModel = JsonSerializer.Deserialize<DbModel>(jsonString) ?? throw new JsonException();
            }
            catch (JsonException)
            {
                File.Delete(FilePath);
            }

            JsonSerializer.Deserialize<DbModel>(jsonString);
        }

        if (!File.Exists(FilePath))
        {
            string jsonString = JsonSerializer.Serialize(dbModel);
            File.WriteAllText(FilePath, jsonString, Encoding.UTF8);
        }

        return dbModel;
    }

    /// <summary>
    /// 存储配置文件
    /// </summary>
    /// <param name="dbModel"></param>
    public static void Save(DbModel dbModel)
    {
        var jsonString = JsonSerializer.Serialize(dbModel);
        File.WriteAllText(FilePath, jsonString, Encoding.UTF8);
    }
}
