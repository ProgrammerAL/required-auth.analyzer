# Required Auth Analyzer

The purpose of this repo is to create a NuGet package which hosts a C# Roslyn Analyzer to call out ASP.NET Core endpoints which do not specify what AuthN/AuthZ is needed for an individual endpoint. The purpose is to force each endpoint to have an attribute specifying the level of AuthN/AuthZ required for said endpoint.

## Some Terms

AuthN = Authentication
AuthZ = Authorization

## Why each endpoint? Why not set it a global flag and change individual endpoints?

Yes, setting a global flag works a lot of the time. But remember, we're humans and we forget things. In small applications, that can work out well. But when you need to scale out to a team, now the entire team needs to remember the rules each time. It's conceptually easier as a developer to see the attribute on the endpoint code to read what the required auth is for that endpoint.

One common solution is to set a global requirement to always require authentication, and then change it only in situation where it should be different. That works well in a simple case, but larger APIs will require a handful of different settings to authentication. You can imagine an application which hosts endpoints for users like: Anonymous, Signed In User, Signed In Admin, Signed In {ROLE NAME HERE}. That last one can have multiple roles. So now developers of that API have to know what the default is, and when to change it to a different requirement.

And you don't want to forget, because there's enough automated tooling out there (for example: https://github.com/amir-hosseinpour/api-authentication-checker) to test which endpoints don't have authentication required, or just don't require the right amout of authentication.



Is it more work? Yeah, you type out 7 more charactrs. Stop being lazy and make your code easier to understand. 


- You prefer the code that way
- 

