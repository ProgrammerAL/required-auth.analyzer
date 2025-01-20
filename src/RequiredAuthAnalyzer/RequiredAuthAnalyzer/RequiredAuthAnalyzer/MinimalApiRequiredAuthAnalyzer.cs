using System;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ProgrammerAL.Analyzers.RequiredAuthAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MinimalApiRequiredAuthAnalyzer : DiagnosticAnalyzer
{
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.MinimalApiRequiredAuthAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.MinimalApiRequiredAuthAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.MinimalApiRequiredAuthAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

    public const string Category = "Security";
    public const string DiagnosticId = "PAL2000";
    public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var invocationExpr = (InvocationExpressionSyntax)context.Node;
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocationExpr.Expression);

        if (symbolInfo.Symbol is IMethodSymbol methodSymbol
            && IsEndpointForMinimalApi(methodSymbol, invocationExpr, context))
        {
            AnalyzeTraditionalControllerEndpoint(methodSymbol, invocationExpr, context);
        }
    }

    private static bool IsEndpointForMinimalApi(IMethodSymbol methodSymbol, InvocationExpressionSyntax invocationExpr, SyntaxNodeAnalysisContext context)
    {
        if (!methodSymbol.Name.StartsWith("Map"))
        {
            return false;
        }

        //Special cases we know aren't specific endpoints
        if (methodSymbol.Name.Equals("MapGroup"))
        {
            return false;
        }

        var parentClass = methodSymbol.ContainingType;
        var isMethodInsideRouteBuilderExtensions = parentClass.ToString().Equals(KnownTypesConstants.EndpointRouteBuilderExtensions);

        return isMethodInsideRouteBuilderExtensions;
    }

    private static void AnalyzeTraditionalControllerEndpoint(IMethodSymbol methodSymbol, InvocationExpressionSyntax methodInvocation, SyntaxNodeAnalysisContext context)
    {
        //The symantec type is the attribute class's constructor
        //  So use the ContainingType to get the full class name
        var authAttributeSymbol = context.Compilation.GetTypeByMetadataName(KnownTypesConstants.AuthorizeAttribute)
                                    ?? throw new System.Exception($"Could not load C# syntax data for attribute {KnownTypesConstants.AuthorizeAttribute}");
        var anonymousAuthAttributeSymbol = context.Compilation.GetTypeByMetadataName(KnownTypesConstants.AllowAnonymousAttribute)
                                    ?? throw new System.Exception($"Could not load C# syntax data for attribute {KnownTypesConstants.AllowAnonymousAttribute}");

        var hasAuthAttribute = methodInvocation.DescendantNodes().OfType<AttributeSyntax>()
            .Any(x =>
            {
                var attributeSymbolInfo = context.SemanticModel.GetSymbolInfo(x);

                return attributeSymbolInfo.Symbol is object
                    && (attributeSymbolInfo.Symbol.ContainingType.IsOrInheritFrom(authAttributeSymbol)
                        || attributeSymbolInfo.Symbol.ContainingType.IsOrInheritFrom(anonymousAuthAttributeSymbol));
            });

        if (!hasAuthAttribute)
        {
            var location = methodInvocation.GetLocation();

            var diagnostic = Diagnostic.Create(Rule, location, methodSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
