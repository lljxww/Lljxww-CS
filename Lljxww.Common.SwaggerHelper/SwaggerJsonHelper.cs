using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Lljxww.Common.SwaggerHelper;

public class SwaggerJsonHelper
{
    public static SwaggerModel? GetSwaggerModel(string jsonText)
    {
        jsonText = jsonText.Replace("$", "");

        JsonObject? jsonObj = JsonNode.Parse(jsonText)?.AsObject();
        if (jsonObj == null)
        {
            return null;
        }

        SwaggerModel? model = JsonSerializer.Deserialize<SwaggerModel>(jsonText,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

        return model;
    }
}