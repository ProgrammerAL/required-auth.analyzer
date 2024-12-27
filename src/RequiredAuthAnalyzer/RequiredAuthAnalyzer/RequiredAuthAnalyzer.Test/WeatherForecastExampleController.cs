using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyNamespace;

[ApiController]
[Route("WeatherForecastExample")]
public class WeatherForecastExampleController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<int> Get()
    {
        return [1, 2, 3];
    }
}
