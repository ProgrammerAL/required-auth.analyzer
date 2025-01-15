using Cake.Common;
using Cake.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ProjectPaths(
    string ProjectName,
    string PathToSln,
    string ProjectFolder,
    string CsprojFile,
    string NuGetCsprojFile,
    string UnitTestProj,
    string OutDir,
    string NuGetFilePath)
{
    public static ProjectPaths LoadFromContext(ICakeContext context, string buildConfiguration, string srcDirectory, string nugetVersion)
    {
        var projectName = "RequiredAuthAnalyzer";
        var codeRootDirectory =  $"{srcDirectory}/{projectName}";
        var pathToSln = $"{codeRootDirectory}/{projectName}.sln";
        var projectDir = $"{codeRootDirectory}/{projectName}";
        var csProjFile = $"{codeRootDirectory}/{projectName}.csproj";
        var nugetCsProjFile = $"{projectDir}/{projectName}.Package/{projectName}.Package.csproj";
        var unitTestsProj = $"{projectDir}/{projectName}.Test/{projectName}.Test.csproj";
        var outDir = projectDir + $"/bin/{buildConfiguration}/cake-build-output";
        var nugetFilePath = outDir + $"/*{nugetVersion}.nupkg";

        return new ProjectPaths(
            projectName,
            pathToSln,
            projectDir,
            csProjFile,
            nugetCsProjFile,
            unitTestsProj,
            outDir,
            nugetFilePath);
    }
};
