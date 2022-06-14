namespace Lljxww.Dapper.Extensions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class LColumnAttribute : Attribute
{
    public LColumnAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}