# CPS Extension Sample
This sample solution can be used as a starting point for extending an existing project type in CPS. The solution contains 2 projects: a vsix, and a nuget package. The goal is to separate your extension into 2 parts. The build logic in the NuGet package, and the design-time (optional) VS features in the vsix. It is recommended that your build logic function without the VSIX being present, so users do not need to include the vsix on build machines. Additionally, the nuget package also contains the capabilities and design time imports, so that your CPS extensions only light up for projects that have the nuget package installed.

With this approach, different templates are not needed for different project types. But rather the base project (eg: .csproj) is used, and the user enables your functionality by simply referencing your nuget package and then optionally installing the vsix.

## CpsExtension.Vsix
This project contains all the optional design-time functionality. This includes design time targets and CPS extension points.

## CpsExtension.NuGet
This project contains all the build-time targets and tools for your extension. It "turns on" the vsix design-time features by being installed to a project, but should not require the vsix to be installed to successfully build.