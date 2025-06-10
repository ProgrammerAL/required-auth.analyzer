# Required Auth Analyzer

This project is for a C# Roslyn Analyzer NuGet to call out ASP.NET Core endpoints which do not specify what AuthN/AuthZ is needed for an individual endpoint. The purpose is to force each endpoint to have an attribute specifying the level of AuthN/AuthZ required for said endpoint.

## Some Terms

AuthN = Authentication
AuthZ = Authorization
Auth = AuthN and AuthZ

## Why each endpoint? 

Yes there are ways to set Auth for endpoints at a global level. And that works a lot of the time. But we're all human, and we forget things. In small applications, this can work out well enough. But when you need to scale out to a team, the entire team needs to remember the rules each time. 

One common solution is to set a global requirement to always require authentication, and then change it only in situation where it should be different. That works well in a simple case, but larger APIs will require different settings for Auth at different endpoints. Imagine an application which hosts endpoints for multiple scenarios. It requires auth at levels like: Anonymous, Signed In User, Signed In Admin, Signed In {ROLE NAME HERE}. So now developers of that API have to know what the default is, and when to change it to a different requirement.

And you don't want to forget, because there's enough automated tooling out there (for example: https://github.com/amir-hosseinpour/api-authentication-checker) to test which endpoints don't have authentication required, or just don't require the right amout of authentication.

As a developer, it's conceptually easier to see the attribute on the endpoint code to know what the required auth is for that endpoint. 

Is it more work? Yeah, a bit. But that's why this Roslyn ANalyzer exists, to make it less work for us.

## How to use this?

It's a Roslyn Analyzer, so all you have to do is add the `ProgrammerAL.Analyzers.ControllerRequiredAuthAnalyzer` NuGet to your `.csproj` file. 

https://www.nuget.org/packages/ProgrammerAL.Analyzers.ControllerRequiredAuthAnalyzer


## What rules does it analyze for?

- PAL2000
  - A Minimal API Endpoint is missing explicit Auth. 
- PAL2001
  - A Controller Endpoint is missing explicit Auth. 

## Can it be less strict?

If you would instead like the option to force Auth at the Controller OR the endpoint level, there is another Roslyn Analyzer hosted at: https://github.com/ProgrammerAL/required-auth.controller.analyzer

