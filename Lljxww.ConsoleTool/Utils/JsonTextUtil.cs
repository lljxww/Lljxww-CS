using System.Text.Json;

namespace Lljxww.ConsoleTool;

public static class JsonTextUtil
{
    public static string JsonPrettify(this string json)
    {
        using var jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }
}
