# CPS Extension Sample
This sample demonstrates:
* Extending an existing project type in CPS
* How to dynamically turn on or off a feature implemented in a vsix by adding and or removing a nuget package that defines a custom `ProjectCapability`
* How to author vsix projects using an SDK-style project which is CPS based (keep in mind that this is not an officially supported scenario currently)
	* Custom rule delivered via a nuget package that provides custom properties for a xaml rule file (`XamlPropertyRule.xaml` defined in `Microsoft.VisualStudio.ProjectSystem.Sdk.Tools`)

## Instructions
1. Open the solution in Visual Studio
2. Create the nuget package
	* In Visual Studio, right click on the `CpsExtension.Nuget` project and select `Pack`
	* This will generate a nuget package: `CpsExtension.Nuget.1.1.0.nupkg` (located in the `bin\debug` subfolder)
2. Build and run the CpsExtension vsix
	* Right click on the `CpsExtension.Vsix` project and select `Set as StartUp Project`
	* Build and run the solution (Ctrl + F5) - this will build the vsix and launch the Visual Studio experimental hive with the vsix installed
3. Create a .Net Core project and install the CpsExtension.Nuget package
	* In the experimental hive launched at previous step, Create a new .Net Core Console App
	* Right click on the project -> Manage Nuget Packages
	* Add the location where `CpsExtension.Nuget.1.1.0.nupkg` was generated at step 1 as a [package source](https://learn.microsoft.com/nuget/tools/package-manager-ui#package-sources)
	* Install the `CpsExtension.Nuget.1.1.0.nupkg` package into your project
4. See the results
	* Once the nuget package was added to the project, you should notice a new debugger named `Custom Debugger` on the main tool bar on the start button
	* Note: the custom debugger is not implemented, so it will report an error if you try to run/debug the project using it
	* If you uninstall the nuget package from the project, the `Custom Debugger` entry will be automatically removed
	* Select a .cs file in the solution explorer and you will see a custom property `Foo Property` available in the properties window

## Implementation notes
This sample solution can be used as a starting point for extending an existing project type in CPS.
The solution contains 2 projects:a vsix, and a nuget package. The goal is to separate your extension into 2 parts. The build logic in the NuGet package, and the design-time (optional) VS features in the vsix. It is recommended that your build logic function without the VSIX being present, so users do not need to include the vsix on build machines. The nuget package also contains the capabilities and design time imports, so that your CPS extensions only light up for projects that have the nuget package installed.

With this approach, different templates are not needed for different project types. But rather the base project (eg: .csproj) is used, and the user enables your functionality by simply referencing your nuget package and then optionally installing the vsix.

Right now, this sample simply adds a debugger to the drop down when the nuget package is installed to a project.

### CpsExtension.Vsix
This project contains all the optional design-time functionality. This includes design time targets and CPS extension points.

The project itself is an sdk-style project changed to build vsix's. This is not an officially supported scenario, but it is possible to get it to work.

The produced VSIX is an all-users vsix that installs the targets and xaml rules to the per-VS-instance MSBuild directory.

When F5'ing the VSIX, the `CpsExtensionDesignTimeTargetsPath` environment variable is set (specified in `launchSettings.json` under the `Properties folder`) that overrides where the targets file from the nuget package picks up the design time targets from the bin directory. This way you can develop without affecting the MSBuild directory for your VS install.

Because this is a CPS based project, you will notice the rich set of properties available when you select a xaml rule file in the solution explorer (`BuildSystem\Rules\CustomDebugger.xaml`). That is because the `Microsoft.VisualStudio.ProjectSystem.Sdk.Tools` package delivers a custom rule `XamlPropertyRule.xaml` that defines these properties. Simply installing and uninstalling the `Microsoft.VisualStudio.ProjectSystem.Sdk` package will turn on or off this behavior.

### CpsExtension.NuGet
This project contains all the build-time targets and tools for your extension. It "turns on" the vsix design-time features by being installed to a project, but should not require the vsix to be installed to successfully build.

While developing on this sample, you will want to add the bin directory of this project, or wherever the nuget package is output to, as a NuGet feed source for your experimental instance. This way you can easily install your nuget package during development for testing.