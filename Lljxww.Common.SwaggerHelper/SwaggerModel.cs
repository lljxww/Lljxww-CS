using System.Text.Json.Serialization;

namespace Lljxww.Common.SwaggerHelper;

public class SwaggerModel
{
    [JsonIgnore] public string Version => Swagger;

    private string Swagger { get; set; }

    public InfoModel Info { get; set; }

    public string Host { get; set; }

    public IList<string> Schemes { get; set; }

    public Dictionary<string, Dictionary<string, MethodInfo>> Paths { get; set; }

    public Dictionary<string, ParamDefinition> Definitions { get; set; }

    public class InfoModel
    {
        public string Version { get; set; }

        public string Title { get; set; }
    }

    public class MethodInfo
    {
        public IList<string> Tags { get; set; }

        public string Summary { get; set; }

        public string OperationId { get; set; }

        public IList<string> Consumes { get; set; }

        public IList<string> Produces { get; set; }

        public IList<Parameter> Parameters { get; set; }

        public Dictionary<string, Response> Responses { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }

        public string In { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public string? Type { get; set; }

        public RequestSchema Schema { get; set; }
    }

    public class RequestSchema
    {
        public string Ref { get; set; }
    }

    public class Response
    {
        public string Description { get; set; }

        public ResponseSchema Schema { get; set; }
    }

    public class ResponseSchema
    {
        public string Type { get; set; }
    }

    public class ParamDefinition
    {
        public string Type { get; set; }

        public Dictionary<string, PropertyDefinition> Properties { get; set; }
    }

    public class PropertyDefinition
    {
        public string? Type { get; set; }

        public string? Format { get; set; }

        public IList<int> Enum { get; set; }

        public bool? ReadOnly { get; set; }

        public Item Items { get; set; }
    }

    public class Item
    {
        public string Format { get; set; }

        public string Type { get; set; }
    }
}