using System.Text.Json;

namespace Lljxww.ConsoleTool;

public class WriteIndentedJsonSerializerOptions
{
    readonly static JsonSerializerOptions instance = new()
    {
        WriteIndented = true
    };

    public static JsonSerializerOptions GetInstance => instance;
}
