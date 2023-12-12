namespace Lljxww.SourceGenerator;

public static class EnumExtension
{
    public static string ToStringFast(this Color color)
    {
        return color switch
        {
            Color.Red => nameof(Color.Red),
            Color.Blue => nameof(Color.Blue),
            _ => color.ToString(),
        };
    }
}
