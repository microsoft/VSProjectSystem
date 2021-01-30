Project Capabilities
=============================================

Please read [about project capabilities](about_project_capabilities.md).

Below is a section for each project capability to describe itself including
what it semantically is intended to mean possibly including examples of projects
known to declare the capability.

| Project capability | Description |
| ------------------ | ----------- |
| VisualC | Project may contain or compile C++ source files.
| CSharp | Project may contain or compile C# source files.
| VB | Project may contain or compile VB source files.
| JavaScript | Project contains and handles JavaScript source files.
| TypeScript | Indicates that the TypeScript build targets are imported into the project and the project is capable of compiling TypeScript source files into JavaScript.
| JavaScriptProject | Indicates that the project supports JavaScript development. This capability also means that the project can manage JavaScript related project items.
| JavaScriptUAPProject |The JavaScript project targets the Universal Windows Platform of Windows 10.
| JavaScriptWindowsPhoneProject | The JavaScript project targets Windows Phone 8.1 platform.
| JavaScriptWindowsAppContainerProject | Indicates that the project is both "JavaScript" and "WindowsAppContainer", so all JavaScript projects that emit *.AppX bundles will declare this capability.
| JavaScriptWJProject | The JavaScript project targets Windows 8.1 desktop platform.
| Cordova | The project integrates with and depends on Apache Cordova.
| FileSystemBasedCordovaProject | Indicates that the Cordova project looks to the file system to determine the project content. This is the defining capability for the project system in Visual Studio Tools for Apache Cordova.
| CPS | Project is based on the Project System Extensibility SDK
| HostSetActiveProjectConfiguration
| ProjectConfigurationsInferredFromUsage
| ProjectConfigurationsDeclaredAsItems
| RunningInVisualStudio
| NotLoadedWithIDEIntegration
| RenameNearbySimilarlyNamedImportsWithProject
| DeclaredSourceItems | Indicates that the project is a typical MSBuild project (not DNX) in that it declares source items in the project itself (rather than a project.json file that assumes all files in the directory are part of a compilation). 
| UserSourceItems | Indicates that the user is allowed to add arbitrary files to their project. 
| SourceItemsFromImports
| AssemblyReferences | Indicates that the project supports assembly references.
| COMReferences | Indicates that the project supports COM references.
| ProjectReferences | Indicates that the project supports project references.
| PackageReferences | Indicates that the project supports package references. For eg: C# projects use PackageReference items for NuGet package references.
| SDKReferences | Indicates that the project supports SDK references.
| SharedProjectReferences | Indicates that the project supports references to Shared Projects.
| WinRTReferences | Indicates that the project references WinRT libraries.
| OutputGroups
| AllTargetOutputGroups
| VisualStudioWellKnownOutputGroups
| SingleFileGenerators
| Managed
| SharedAssetsProject
| SharedImports
| NestedProjects
| WindowsXaml
| WindowsXamlPage
| WindowsXamlCodeBehind
| WindowsXamlResourceDictionary
| WindowsXamlUserControl
| RelativePathDerivedDefaultNamespace | *DEPRECATED* This is an example of a BAD project capability because it merely exists to activate a design-time experience and does not reflect a genuine capability of the project.  This should be a project property instead.
| ReferencesFolder | *DEPRECATED* This is an example of a BAD project capability because it merely exists to activate a design-time experience and does not reflect a genuine capability of the project.  If worth keeping at all, this should be a project property instead.
| LanguageService | *DEPRECATED* This is an example of a BAD project capability because it merely exists to activate a design-time experience and does not reflect a genuine capability of the project.  If worth keeping at all, this should be a project property instead.
| BuildWindowsDesktopTarget
| PerPlatformCompilation (aka. #ifdef capability) | Indicates that items in this project are compiled individually for every target platform.
| MultiTarget | Indicates that items in this project may be applied to multiple platforms.
| DeploymentProject | Indicates a project that is capable of provisioning Azure resources using Azure Resource Manager that will create an environment for an application.
| FabricApplication | Indicates a project that represents a Service Fabric application.
| LSJavaScript.v45 | Indicates a project that represents a LightSwitch JavaScript V4.5 project.
| FolderPublish | Indicates a project that is capable of publishing the deployment artifacts to a folder.
| TestContainer | The project may contain unit tests. 
| AppDesigner | Indicates that the project uses the app designer for managing project properties.
| HandlesOwnReload | Indicates that the project can handle a reload by itself (potentially smartly) without the solution doing a full reload of the project when the project file changes on disk.
| UseFileGlobs | Indicates that the project file can include files using MSBuild file globbing patterns.
| EditAndContinue | Indicates that the project supports the edit and continue debugging feature.
| OpenProjectFile | Indicates that the project is capable of handling the project file being edited live in an IDE while the project is already loaded.
| DependenciesTree | Indicates that the project supports the dependencies node in Visual Studio.
| LaunchProfiles | Indicates that the project supports multiple profiles for debugging.
| ReferenceManagerAssemblies | Indicates that the project will show the Assemblies tab in the Reference manager dialog in Visual Studio.
| ReferenceManagerBrowse | Indicates that the project will show the Browse button in the Reference manager dialog in Visual Studio.
| ReferenceManagerCOM | Indicates that the project will show the COM tab in the Reference manager dialog in Visual Studio.
| ReferenceManagerProjects | Indicates that the project will show the Projects tab in the Reference manager dialog in Visual Studio.
| ReferenceManagerSharedProjects | Indicates that the project will show the Shared Projects tab in the Reference manager dialog in Visual Studio.
| ReferenceManagerWinRT | Indicates that the project will show the WinRT tab in the Reference manager dialog in Visual Studio.
| NoGeneralDependentFileIcon | Indicates that dependent files should have their own corresponding icons instead of generic one


## Summary table

Below is a table that summarizes each capability and popular project types
they are declared in:

| Legend |                                                                                        |
|:------:|----------------------------------------------------------------------------------------|
|   U    | Unconfigured Project scope (capabilities appear in Unconfigured and Configured scopes) |
|   C    | Configured Project scope only                                                          |
|  Bold  | already shipped                                                                        |

## Additional discussion

#### PerPlatformCompilation

Defined by: Shared projects 

Summary: Indicates that items in this project are compiled individually
for every target platform.

What about when/if XAML gains the ability to have per-platform (or
per-form factor) XAML sections? This might be done at runtime or
compile time perhaps. We might want to upfront define whether either
of these future mechanisms would merit this capability or a new one.

Answer: PerPlatformXamlCompilation may come later.
    
#### MultiTarget (aka. Multiplat capability)

Defined by: PCLs and Shared Projects

Summary: Indicates that items in this project may be applied to multiple
platforms.

This capability is only present for PCLs that actually target > 1
platform (not a PCL that targets 1).

