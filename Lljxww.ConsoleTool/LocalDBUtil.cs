using System.Text;
using System.Text.Json;

namespace Lljxww.ConsoleTool;

public class LocalDBUtil
{
    public static string Get(string key)
    {
        var dbModel = Init();

        if (dbModel.Settings.TryGetValue(key, out string value))
        {
            return value;
        }

        return default;
    }

    public static void Set(string key, string value)
    {
        var dbModel = Init();
        dbModel.Settings.Add(key, value);
        Save(dbModel);
    }

    public static string ConfigPath => Init().CallerConfigPath;

    private static string FileDirectory => Path.Combine(Environment.CurrentDirectory, "config");
    private static string FilePath => Path.Combine(FileDirectory, "setting.json");

    private static DbModel Init()
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

    private static void Save(DbModel dbModel)
    {
        var jsonString = JsonSerializer.Serialize(dbModel);
        File.WriteAllText(FilePath, jsonString, Encoding.UTF8);
    }
}
