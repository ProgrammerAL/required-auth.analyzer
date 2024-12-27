using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RequiredAuthAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MinimalApiRequiredAuthAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MinimalApiRequiredAuthAnalyzerAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Security";

#pragma warning disable RS2008 // Enable analyzer release tracking
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
#pragma warning restore RS2008 // Enable analyzer release tracking

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var invocationExpr = (InvocationExpressionSyntax)context.Node;
            var memberAccessExpr = invocationExpr.Expression as MemberAccessExpressionSyntax;

            var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccessExpr);
            var methodCalledSymbol = symbolInfo.Symbol as IMethodSymbol;

            if (methodCalledSymbol == null)
            {
                return;
            }

            if (IsEndpointForMinimalApi(methodCalledSymbol))
            {
                AnalyzeTraditionalControllerEndpoint(methodCalledSymbol, context);
            }
        }

        private static bool IsEndpointForMinimalApi(IMethodSymbol methodSymbol)
        {
            var parentClass = methodSymbol.ContainingType;
            var isMethodInsideRouteBuilderExtensions = parentClass.Name.Equals("Microsoft.AspNetCore.Builder.EndpointRouteBuilderExtensions");

            return isMethodInsideRouteBuilderExtensions;
        }

        private static void AnalyzeTraditionalControllerEndpoint(IMethodSymbol methodSymbol, SyntaxNodeAnalysisContext context)
        {
            //TODO: Are there more attributes?
            var hasAuthAttribute = methodSymbol.GetAttributes()
                .Any(x =>
                {
                    var name = x.GetType().FullName;
                    return name == "Microsoft.AspNetCore.Authorization.AuthorizeAttribute"
                        || name == "Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute";
                });

            if (!hasAuthAttribute)
            {
                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
