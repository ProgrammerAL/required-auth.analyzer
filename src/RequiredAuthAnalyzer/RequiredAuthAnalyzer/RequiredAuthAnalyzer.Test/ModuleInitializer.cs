using System.Runtime.CompilerServices;

using Gu.Roslyn.Asserts;

namespace RequiredAuthAnalyzer.Test;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        Settings.Default = Settings.Default.WithMetadataReferences(
            MetadataReferences.Transitive(
                typeof(Microsoft.Extensions.Hosting.GenericHostBuilderExtensions),
                typeof(Microsoft.Extensions.DependencyInjection.MvcServiceCollectionExtensions),
                typeof(Microsoft.AspNetCore.Http.TypedResults),
                typeof(Microsoft.AspNetCore.Builder.HttpsPolicyBuilderExtensions)));
    }
}
