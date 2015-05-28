Project Capabilities
=============================================

Please read [about project capabilities](about_project_capabilities.md).

Below is a section for each project capability to describe itself including
what it semantically is intended to mean possibly including examples of projects
known to declare the capability.

### Languages

#### VisualC

#### CSharp

#### VB

### Systems

#### CPS

#### HostSetActiveProjectConfiguration

#### ProjectConfigurationsInferredFromUsage

#### ProjectConfigurationsDeclaredAsItems

#### RunningInVisualStudio

#### NotLoadedWithIDEIntegration

#### RenameNearbySimilarlyNamedImportsWithProject

### Source items

#### DeclaredSourceItems

#### UserSourceItems

#### SourceItemsFromImports


### References

#### AssemblyReferences
Indicates that the project supports assembly references.

#### COMReferences
Indicates that the project supports COM references.

#### ProjectReferences
Indicates that the project supports project references.

#### SDKReferences

#### SharedProjectReferences
Indicates that the project supports references to Shared Projects.

#### WinRTReferences

### Output groups

#### OutputGroups
TODO

#### AllTargetOutputGroups
TODO

#### VisualStudioWellKnownOutputGroups
TODO

### Designer activations

#### SingleFileGenerators
TODO

#### Managed
TODO

#### SharedAssetsProject
TODO

#### SharedImports

#### NestedProjects
TODO

#### WindowsXaml

#### WindowsXamlPage

#### WindowsXamlCodeBehind

#### WindowsXamlResourceDictionary

#### WindowsXamlUserControl

#### RelativePathDerivedDefaultNamespace
*DEPRECATED*

This is an example of a BAD project capability because it merely exists
to activate a design-time experience and does not reflect a genuine
capability of the project. 
This should be a project property instead.

#### ReferencesFolder
*DEPRECATED*

This is an example of a BAD project capability because it merely exists
to activate a design-time experience and does not reflect a genuine
capability of the project. 
If worth keeping at all, this should be a project property instead.

#### LanguageService
*DEPRECATED*

This is an example of a BAD project capability because it merely exists
to activate a design-time experience and does not reflect a genuine
capability of the project. 
If worth keeping at all, this should be a project property instead.

### Multi-platform targeting

#### BuildWindowsDesktopTarget


#### PerPlatformCompilation (aka. #ifdef capability)

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


## Summary table

Below is a table that summarizes each capability and popular project types
they are declared in:

| Legend |                                                                                        |
|:------:|----------------------------------------------------------------------------------------|
|   U    | Unconfigured Project scope (capabilities appear in Unconfigured and Configured scopes) |
|   C    | Configured Project scope only                                                          |
|  Bold  | already shipped                                                                        |

