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
    public class RequiredAuthAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RequiredAuthAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

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
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;
            if (IsEndpointForTraditionalController(methodSymbol))
            {
                AnalyzeTraditionalControllerEndpoint(methodSymbol, context);
            }


            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeTraditionalControllerEndpoint(IMethodSymbol methodSymbol, SymbolAnalysisContext context)
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

        private static bool IsEndpointForTraditionalController(IMethodSymbol methodSymbol)
        {
            var parentClass = methodSymbol.ContainingType;
            var isParentApiController = parentClass
                    .GetAttributes()
                    .Any(x => x.GetType().FullName == "Microsoft.AspNetCore.Mvc.ApiControllerAttribute");

            if (!isParentApiController)
            {
                return false;
            }

            var isMethodAnEndpoint = methodSymbol.DeclaredAccessibility == Accessibility.Public
                && methodSymbol.IsStatic == false
                && methodSymbol.MethodKind == MethodKind.Ordinary;

            return isMethodAnEndpoint;
        }
    }
}
