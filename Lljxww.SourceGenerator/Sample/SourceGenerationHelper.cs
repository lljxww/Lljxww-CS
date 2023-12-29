using System.Text;

namespace Lljxww.SourceGenerator.Sample;

public static class SourceGenerationHelper
{
    public const string Attribute = @"
namespace Lljxww.SourceGenerator {
    [System.AttributeUsage(System.AttributeTargets.Enum)]
    public class EnumExtensionsAttribute : System.Attribute
    {
    }
}
";

    public static string GenerateExtensionClass(List<EnumToGenerate> enumsToGenerate)
    {
        StringBuilder sb = new();
        _ = sb.Append(@"
namespace Lljxww.SourceGenerator 
{
    public static partial class EnumExtensions
    {");

        foreach (EnumToGenerate enumToGenerate in enumsToGenerate)
        {
            _ = sb.Append(@"
        public static string ToStringFast(this ").Append(enumToGenerate.Name)
        .Append(@" value)
            => value switch
            {");

            foreach (string member in enumToGenerate.Values)
            {
                _ = sb.Append(@"
                    ").Append(enumToGenerate.Name).Append('.').Append(member)
                    .Append(" => nameof(")
                    .Append(enumToGenerate.Name).Append('.').Append(member).Append("),");
            }

            _ = sb.Append(@"
                        _ => value.ToString(),
                    };
");
        }

        _ = sb.Append(@"
        }
}");

        return sb.ToString();
    }
}
