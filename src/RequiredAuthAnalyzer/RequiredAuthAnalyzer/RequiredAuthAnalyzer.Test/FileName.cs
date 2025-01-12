using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace RequiredAuthAnalyzer.Test;

public class MyEndpoint
{
    public static IEndpointConventionBuilder RegisterApiEndpoint(RouteGroupBuilder group)
    {
        return group.MapPost("/my-endpoint",
        () =>
        {
            return TypedResults.Ok();
        });
    }
}
