using System.Text;
using System.Text.Json;

namespace Lljxww.ConsoleTool;

public class DbModelUtil
{
    public static DbModel Instance { get; private set; }

    static DbModelUtil()
    {
        Instance = Init();
    }

    /// <summary>
    /// 初始化本地文件
    /// </summary>
    /// <returns></returns>
    internal static DbModel Init()
    {
        if (!Directory.Exists(PathUtil.FileDirectory))
        {
            Directory.CreateDirectory(PathUtil.FileDirectory);
        }

        DbModel dbModel = new();

        if (File.Exists(PathUtil.FilePath))
        {
            var jsonString = File.ReadAllText(PathUtil.FilePath, Encoding.UTF8);
            try
            {
                dbModel = JsonSerializer.Deserialize<DbModel>(jsonString) ?? throw new JsonException();
            }
            catch (JsonException)
            {
                File.Delete(PathUtil.FilePath);
            }

            JsonSerializer.Deserialize<DbModel>(jsonString);
        }

        if (!File.Exists(PathUtil.FilePath))
        {
            string jsonString = JsonSerializer.Serialize(dbModel);
            File.WriteAllText(PathUtil.FilePath, jsonString, Encoding.UTF8);
        }

        return dbModel;
    }

    /// <summary>
    /// 存储配置文件
    /// </summary>
    /// <param name="dbModel"></param>
    internal static void Save2File(Func<DbModel, DbModel> editAction)
    {
        if (editAction == null)
        {
            return;
        }

        Instance = editAction.Invoke(Instance);
        var jsonString = JsonSerializer.Serialize(Instance);
        File.WriteAllText(PathUtil.FilePath, jsonString, Encoding.UTF8);
    }
}
