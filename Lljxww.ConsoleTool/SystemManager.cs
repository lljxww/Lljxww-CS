using System.Text;
using System.Text.Json;

namespace Lljxww.ConsoleTool;

public static partial class SystemManager
{
    static SystemManager()
    {
        dbModel = GetDbModelFromFile();
    }

    private static DbModel dbModel { get; set; }

    public static ActionResult SetCallerConfigPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return new ActionResult
            {
                Success = false,
                Message = $"指定的路径不能为空: {path}"
            };
        }

        if (!File.Exists(path))
        {
            return new ActionResult
            {
                Success = false,
                Message = $"指定的文件不存在: {path}"
            };
        }

        dbModel.CallerConfigPath = path;
        Save2File(dbModel);

        return new ActionResult
        {
            Success = true
        };
    }

    public static string GetCallerConfigPath() => dbModel.CallerConfigPath;
}

public static partial class SystemManager
{
    /// <summary>
    /// 程序数据文件的路径
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
    private static DbModel GetDbModelFromFile()
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
    private static void Save2File(DbModel dbModel)
    {
        var jsonString = JsonSerializer.Serialize(dbModel);
        File.WriteAllText(FilePath, jsonString, Encoding.UTF8);
    }
}