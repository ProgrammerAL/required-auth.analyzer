using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ProgrammerAL.Analyzers.RequiredAuthAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ControllerRequiredAuthAnalyzer : DiagnosticAnalyzer
{
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ControllerRequiredAuthAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ControllerRequiredAuthAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ControllerRequiredAuthAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

    public const string Category = "Security";
    public const string DiagnosticId = "PAL2001";
    public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        var methodSymbol = (IMethodSymbol)context.Symbol;
        if (IsEndpointForTraditionalController(methodSymbol, context))
        {
            AnalyzeTraditionalControllerEndpoint(methodSymbol, context);
        }
    }

    private static bool IsEndpointForTraditionalController(IMethodSymbol methodSymbol, SymbolAnalysisContext context)
    {
        var parentClass = methodSymbol.ContainingType;

        var isParentApiController = parentClass
                .GetAttributes()
                .Any(x => x.AttributeClass?.ToString() == KnownTypesConstants.ApiControllerAttribute);

        if (!isParentApiController)
        {
            return false;
        }

        var isMethodAnEndpoint = methodSymbol.DeclaredAccessibility == Accessibility.Public
            && methodSymbol.IsStatic == false
            && methodSymbol.MethodKind == MethodKind.Ordinary;

        return isMethodAnEndpoint;
    }

    private static void AnalyzeTraditionalControllerEndpoint(IMethodSymbol methodSymbol, SymbolAnalysisContext context)
    {
        //The symantec type is the attribute class's constructor
        //  So use the ContainingType to get the full class name
        var authAttributeSymbol = context.Compilation.GetTypeByMetadataName(KnownTypesConstants.AuthorizeAttribute) 
                                    ?? throw new System.Exception($"Could not load C# syntax data for attribute {KnownTypesConstants.AuthorizeAttribute}");
        var anonymousAuthAttributeSymbol = context.Compilation.GetTypeByMetadataName(KnownTypesConstants.AllowAnonymousAttribute) 
                                    ?? throw new System.Exception($"Could not load C# syntax data for attribute {KnownTypesConstants.AllowAnonymousAttribute}");
        
        var hasAuthAttribute = methodSymbol.GetAttributes()
            .Any(x =>
            {
                return x.AttributeClass is object
                    && (x.AttributeClass.IsOrInheritFrom(authAttributeSymbol)
                        || x.AttributeClass.IsOrInheritFrom(anonymousAuthAttributeSymbol));
            });

        if (!hasAuthAttribute)
        {
            var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }

}
