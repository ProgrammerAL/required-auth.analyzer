using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAL.Analyzers.RequiredAuthAnalyzer;

public static class KnownTypesConstants
{
    public const string AuthorizeAttribute = "Microsoft.AspNetCore.Authorization.AuthorizeAttribute";
    public const string AllowAnonymousAttribute = "Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute";


    public const string EndpointRouteBuilderExtensions = "Microsoft.AspNetCore.Builder.EndpointRouteBuilderExtensions";
    public const string ApiControllerAttribute = "Microsoft.AspNetCore.Mvc.ApiControllerAttribute";
}
