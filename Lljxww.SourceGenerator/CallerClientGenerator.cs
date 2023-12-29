using Lljxww.ApiCaller.Models.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lljxww.SourceGenerator
{
    [Generator]
    public class CallerClientGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)
                ).Where(static m => m is not null);
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
        {
            return false;
        }

        private static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
