using System.Text.Json.Serialization;

namespace Lljxww.Common.SwaggerHelper;

public class SwaggerModel2 : SwaggerModel
{
    [JsonIgnore] public override string Version => Swagger;

    private string Swagger { get; set; }

    public InfoModel2 Info { get; set; }

    public string Host { get; set; }

    public IList<string> Schemes { get; set; }

    public Dictionary<string, Dictionary<string, MethodInfo2>> Paths { get; set; }

    public Dictionary<string, ParamDefinition> Definitions { get; set; }

    public class InfoModel2
    {
        public string Version { get; set; }

        public string Title { get; set; }
    }

    public class MethodInfo2
    {
        public IList<string> Tags { get; set; }

        public string Summary { get; set; }

        public string OperationId { get; set; }

        public IList<string> Consumes { get; set; }

        public IList<string> Produces { get; set; }

        public IList<Parameter2> Parameters { get; set; }

        public Dictionary<string, Response2> Responses { get; set; }
    }

    public class Parameter2
    {
        public string Name { get; set; }

        public string In { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public string? Type { get; set; }

        public Dictionary<string, ParamDefinition> Schema { get; set; }
    }

    public class RequestSchema2
    {
        [JsonPropertyName("$ref")] public PropertyDefinition Ref { get; set; }
    }

    public class Response2
    {
        public string Description { get; set; }

        public ResponseSchema2 Schema { get; set; }
    }

    public class ResponseSchema2
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

        public Item2 Items { get; set; }
    }

    public class Item2
    {
        public string Format { get; set; }

        public string Type { get; set; }
    }
}