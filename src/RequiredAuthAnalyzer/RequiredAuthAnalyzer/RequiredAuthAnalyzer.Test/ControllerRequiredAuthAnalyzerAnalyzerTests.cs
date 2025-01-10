using System.Threading.Tasks;

using Gu.Roslyn.Asserts;

using Microsoft;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//using VerifyCS = RequiredAuthAnalyzer.Test.Verifiers.CSharpAnalyzerVerifier<
//    RequiredAuthAnalyzer.ControllerRequiredAuthAnalyzerAnalyzer>;

namespace RequiredAuthAnalyzer.Test;

[TestClass]
public class ControllerRequiredAuthAnalyzerAnalyzerTests
{
    private static readonly DiagnosticAnalyzer Analyzer = new ControllerRequiredAuthAnalyzerAnalyzer();
    private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(ControllerRequiredAuthAnalyzerAnalyzer.Rule);

    //No diagnostics expected to show up
    [TestMethod]
    public void WhenCodeEmpty_AssertNoDiagnostic()
    {
        var code = @"";
        RoslynAssert.Valid(Analyzer, code);
    }


    [TestMethod]
    [DataRow("Authorize")]
    [DataRow("AuthorizeAttribute")]
    [DataRow("AllowAnonymous")]
    [DataRow("AllowAnonymousAttribute")]
    public void WhenAllowAnonymous_AssertNoDiagnostic(string attributeName)
    {
        var code = $$"""
        using System.Collections.Generic;
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Mvc;

        namespace MyNamespace;

        [ApiController]
        [Route("WeatherForecast")]
        public class WeatherForecastController : ControllerBase
        {
            [{{attributeName}}]
            [HttpGet]
            public IEnumerable<int> Get()
            {
                return [1, 2, 3];
            }
        }
    """;

        RoslynAssert.Valid(Analyzer, code);
    }

    [TestMethod]
    public void WhenNoAuthAttribute_AssertDiagnostic()
    {
        var code = @"
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    namespace MyNamespace;

    [Microsoft.AspNetCore.Mvc.ApiController]
    [Route(""[controller]"")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet(Name = ""GetWeatherForecast"")]
        public IEnumerable<int> Get()
        {
            return new[]{1, 2, 3};
        }
    }    
    ";

        RoslynAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
    }
}
