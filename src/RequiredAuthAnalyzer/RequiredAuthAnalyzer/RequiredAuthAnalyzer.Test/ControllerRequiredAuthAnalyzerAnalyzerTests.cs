using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using VerifyCS = RequiredAuthAnalyzer.Test.Verifiers.CSharpAnalyzerVerifier<
    RequiredAuthAnalyzer.ControllerRequiredAuthAnalyzerAnalyzer>;

namespace RequiredAuthAnalyzer.Test;

[TestClass]
public class ControllerRequiredAuthAnalyzerAnalyzer
{
    //No diagnostics expected to show up
    [TestMethod]
    public async Task WhenEmpty_AssertNoDiagnostic()
    {
        var test = @"";

        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    [TestMethod]
    public async Task WhenAllowAnonymous_AssertNoDiagnostic()
    {
        var test = @"
        using System.Collections.Generic;
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Mvc;

        namespace MyNamespace;

        [ApiController]
        [Route(""WeatherForecast"")]
        public class WeatherForecastController : ControllerBase
        {
            [AllowAnonymous]
            [HttpGet]
            public IEnumerable<int> Get()
            {
                return [1, 2, 3];
            }
        }
    ";

        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    [TestMethod]
    public async Task WhenMissingAuthAttribute_AssertDiagnostic()
    {
        var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace MyNamespace;

    [ApiController]
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

        var expected = VerifyCS.Diagnostic("ControllerRequiredAuthAnalyzerAnalyzer").WithLocation(0).WithArguments("TypeName");
        await VerifyCS.VerifyAnalyzerAsync(test, expected);
    }
}
