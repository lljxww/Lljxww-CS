using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Lljxww.Common.SourceGen
{
    public class EnumValidator : ISourceGenerator
    {
        public EnumValidator()
        {
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            throw new NotImplementedException();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            string sourceBuilder = @"
using System;

namespace Lljxww.Common.SourceGen;

public static class SourceGenDemo
{
    public static void SayHello()
    {
        Console.WriteLine(""Hello from SourceGenDemo"");
    }
}
";
            context.AddSource("SourceGenDemo", SourceText.From(sourceBuilder, Encoding.UTF8));
        }
    }
}