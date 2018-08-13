# CPS Extension Sample
This sample solution can be used as a starting point for extending an existing project type in CPS. The solution contains 2 projects: a vsix, and a nuget package. The goal is to separate your extension into 2 parts. The build logic in the NuGet package, and the design-time (optional) VS features in the vsix. It is recommended that your build logic function without the VSIX being present, so users do not need to include the vsix on build machines. The nuget package also contains the capabilities and design time imports, so that your CPS extensions only light up for projects that have the nuget package installed.

With this approach, different templates are not needed for different project types. But rather the base project (eg: .csproj) is used, and the user enables your functionality by simply referencing your nuget package and then optionally installing the vsix.

Right now, this sample simply adds a debugger to the drop down when the nuget package is installed to a project.

## CpsExtension.Vsix
This project contains all the optional design-time functionality. This includes design time targets and CPS extension points.

The project itself is an sdk style project changed to build vsix's. This is not an officially supported scenario, but it is possible to get it to work.

The produced VSIX is an all-users vsix that installs the targets and xaml rules to the per-VS-instance MSBuild directory. When F5'ing the VSIX, an environment variable is set that overrides where the nuget package targets to pick up the design time targets from the bin directory. This way you can develop without affecting the MSBuild directory for your VS install.

## CpsExtension.NuGet
This project contains all the build-time targets and tools for your extension. It "turns on" the vsix design-time features by being installed to a project, but should not require the vsix to be installed to successfully build.

While developing on this sample, you will want to add the bin directory of this project, or wherever the nuget package is output to, as a NuGet feed source for your experimental instance. This way you can easily install your nuget package during development for testing.