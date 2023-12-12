namespace Lljxww.SourceGenerator;

public readonly struct EnumToGenerate(string name, List<string> values)
{
    public readonly string Name = name;

    public readonly List<string> Values = values;
}
