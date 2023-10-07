using System.Text.Json;

namespace Lljxww.ConsoleTool.Utils;

public static class JsonTextUtil
{
    public static string JsonPrettify(this string json)
    {
        using JsonDocument jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }
}
