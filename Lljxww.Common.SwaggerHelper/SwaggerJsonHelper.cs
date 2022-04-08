using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Lljxww.Common.SwaggerHelper;

public class SwaggerJsonHelper
{
    public static SwaggerModel? GetSwaggerModel(string jsonText)
    {
        JsonObject? jsonObj = JsonNode.Parse(jsonText)?.AsObject();
        if (jsonObj == null)
        {
            return null;
        }

        string version = jsonObj.ContainsKey("swagger")
            ? jsonObj["swagger"]!.ToString()
            : jsonObj.ContainsKey("openapi")
                ? jsonObj["openapi"]!.ToString()
                : "-1.0";

        SwaggerModel? model = version.Split('.')[0] switch
        {
            "2" => JsonSerializer.Deserialize<SwaggerModel2>(jsonText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            }),
            _ => throw new NotImplementedException("Unsupported version")
        };

        return model;
    }
}