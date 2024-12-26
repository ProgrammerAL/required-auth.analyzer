# Required Auth Analyzer

The purpose of this repo is to create a NuGet package which hosts a C# Roslyn Analyzer to remind code authors to use an attribute to specify what authentication/authorization is needed for an individual endpoint.

## Some Terms

AuthN = Authentication
AuthZ = Authorization

## Why not set it project wide and change it only when it's needed?

Because we're human and we forget. There are different ways you can manage what AuthN/AuthZ is used for ASP.NET Endpoints, but it generally comes down to "Set a base rule rule globally, and change it in the individual endpoints that should be different".

In small applications, that can work out well. But when you need to scale out to a team, now the entire team needs to remember the rules each time. It's conceptually easier as a developer to see the attribute on the endpoint and know what is required for an end user to use that endpoint.

Is it more work? Yeah, you type out 7 more charactrs. Stop being lazy and make your code easier to understand. 


