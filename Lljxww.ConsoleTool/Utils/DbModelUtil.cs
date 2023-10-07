using System.Text;
using System.Text.Json;

namespace Lljxww.ConsoleTool.Utils;

internal class DbModelUtil
{
    private static readonly string VERSION = "0.2";

    internal static DbModel Instance { get; private set; }

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
        if (!Directory.Exists(PathUtil.AppConfigFileDirectory))
        {
            _ = Directory.CreateDirectory(PathUtil.AppConfigFileDirectory);
        }

        DbModel dbModel = new();

        if (File.Exists(PathUtil.DbModelFilePath))
        {
            string jsonString = File.ReadAllText(PathUtil.DbModelFilePath, Encoding.UTF8);
            try
            {
                dbModel = JsonSerializer.Deserialize<DbModel>(jsonString) ?? throw new JsonException();

                // 验证版本
                if (!string.Equals(VERSION, dbModel.Version))
                {
                    File.Delete(PathUtil.DbModelFilePath);
                }
            }
            catch (JsonException)
            {
                File.Delete(PathUtil.DbModelFilePath);
            }

            _ = JsonSerializer.Deserialize<DbModel>(jsonString);
        }

        if (!File.Exists(PathUtil.DbModelFilePath))
        {
            string jsonString = JsonSerializer.Serialize(dbModel);
            File.WriteAllText(PathUtil.DbModelFilePath, jsonString, Encoding.UTF8);
        }

        return dbModel;
    }

    /// <summary>
    /// 存储配置文件
    /// </summary>
    /// <param name="dbModel"></param>
    internal static void UpdateDbModel(Func<DbModel, DbModel> editAction)
    {
        if (editAction == null)
        {
            return;
        }

        Instance = editAction.Invoke(Instance);
        string jsonString = JsonSerializer.Serialize(Instance);
        File.WriteAllText(PathUtil.DbModelFilePath, jsonString, Encoding.UTF8);
    }
}
