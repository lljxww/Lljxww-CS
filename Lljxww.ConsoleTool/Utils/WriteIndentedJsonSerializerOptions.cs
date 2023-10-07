using System.Text.Json;

namespace Lljxww.ConsoleTool.Utils;

public class WriteIndentedJsonSerializerOptions
{
    public static JsonSerializerOptions GetInstance { get; } = new()
    {
        WriteIndented = true
    };
}
