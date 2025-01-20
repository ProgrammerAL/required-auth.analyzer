using Gu.Roslyn.Asserts;

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProgrammerAL.Analyzers.RequiredAuthAnalyzer;

namespace RequiredAuthAnalyzer.Test;

[TestClass]
public class MinimalApiRequiredAuthAnalyzerAnalyzerTests
{
    private static readonly DiagnosticAnalyzer Analyzer = new MinimalApiRequiredAuthAnalyzer();
    private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(MinimalApiRequiredAuthAnalyzer.Rule);

    [TestMethod]
    public void WhenCodeEmpty_AssertNoRuleViolation()
    {
        var code = @"";
        RoslynAssert.Valid(Analyzer, code);
    }

    [TestMethod]
    [DataRow("Authorize")]
    [DataRow("AuthorizeAttribute")]
    [DataRow("AllowAnonymous")]
    [DataRow("AllowAnonymousAttribute")]
    public void WhenSpecifyingAuthWithShortName_AssertNoRuleViolation(string attributeName)
    {
        var code = $$"""
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Http;
        using Microsoft.AspNetCore.Routing;
        using Microsoft.AspNetCore.Authorization;

        namespace RequiredAuthAnalyzer.Test;

        public class MyEndpoint
        {
            public static IEndpointConventionBuilder RegisterApiEndpoint(RouteGroupBuilder group)
            {
                return group.MapGet("/my-endpoint",
                [{{attributeName}}] () =>
                {
                    return TypedResults.Ok();
                });
            }
        }
    """;

        RoslynAssert.Valid(Analyzer, code);
    }

    [TestMethod]
    [DataRow("Microsoft.AspNetCore.Authorization.Authorize")]
    [DataRow("Microsoft.AspNetCore.Authorization.AuthorizeAttribute")]
    [DataRow("Microsoft.AspNetCore.Authorization.AllowAnonymous")]
    [DataRow("Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute")]
    public void WhenSpecifyingAuthWithFullyQualifiedName_AssertNoRuleViolation(string attributeName)
    {
        var code = $$"""
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Http;
        using Microsoft.AspNetCore.Routing;
    
        namespace RequiredAuthAnalyzer.Test;
    
        public class MyEndpoint
        {
            public static IEndpointConventionBuilder RegisterApiEndpoint(RouteGroupBuilder group)
            {
                return group.MapGet("/my-endpoint",
                [{{attributeName}}] () =>
                {
                    return TypedResults.Ok();
                });
            }
        }
    """;

        RoslynAssert.Valid(Analyzer, code);
    }

    [TestMethod]
    public void WhenNoAuthAttribute_AssertRuleViolation()
    {
        var code = @"
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    namespace RequiredAuthAnalyzer.Test;

    public class MyEndpoint
    {
        public static IEndpointConventionBuilder RegisterApiEndpoint(RouteGroupBuilder group)
        {
            return group.MapGet(""/my-endpoint"",
            () =>
            {
                return TypedResults.Ok();
            });
        }
    }  
    ";

        RoslynAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
    }

    [TestMethod]
    public void WhenSpecifyingAuthWithInheritingAttribute_AssertNoRuleViolation()
    {
        var code = """
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Http;
        using Microsoft.AspNetCore.Routing;
    
        namespace RequiredAuthAnalyzer.Test;
    
        public class MyAuthAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
        {
        }

        public class MyEndpoint
        {
            public static IEndpointConventionBuilder RegisterApiEndpoint(RouteGroupBuilder group)
            {
                return group.MapGet("/my-endpoint",
                [MyAuthAttribute] () =>
                {
                    return TypedResults.Ok();
                });
            }
        }
    """;

        RoslynAssert.Valid(Analyzer, code);
    }


    [TestMethod]
    public void WhenMappingGroup_AssertNoRuleViolation()
    {
        var code = @"
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Http;
        using Microsoft.AspNetCore.Routing;

        namespace RequiredAuthAnalyzer.Test;

        public class MyEndpoint
        {
            public static IEndpointConventionBuilder RegisterApiEndpoint(WebApplication app)
            {
                var group = app.MapGroup(""/my-api/my-group"");

                group.MapGet(""/my-endpoint-1"",
                [Microsoft.AspNetCore.Authorization.Authorize] () =>
                {
                    return TypedResults.Ok();
                });

                group.MapGet(""/my-endpoint-2"",
                [Microsoft.AspNetCore.Authorization.AllowAnonymous]() =>
                {
                    return TypedResults.Ok();
                });

                return group;
            }
        }  
    ";

        RoslynAssert.Valid(Analyzer, code);
    }
}
