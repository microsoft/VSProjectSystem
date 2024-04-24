# Project Capabilities

Project capabilities are the recommended way to determine the type, platform,
and features of a project. Simply checking the language or the project
file extension does not help if you want to check for WPF vs. WinForms
vs. Windows 8 XAML, for example. There are a great many different aspects
to a project that may be present regardless of language. Do you want code
that runs against any Windows 8 targeting project regardless of language?
Do you want to target just JavaScript Win8 projects but not LightSwitch
JS projects? Project capability checks are the answer.

## Checking capabilities

The presence of some capability can be detected on a given project with
code such as:

```csharp
using Microsoft.VisualStudio.Shell; // imports the extension method

IVsHierarchy hierarchy;
bool match = hierarchy.IsCapabilityMatch("SomeCapability");
```

Where [`IVsHierarchy.IsCapabilityMatch`](https://learn.microsoft.com/dotnet/api/microsoft.visualstudio.shell.packageutilities.iscapabilitymatch) is an extension method.

Project capability expressions can also be passed to the `IsCapabilityMatch`
method in order to test for combinations of capabilities (including
AND, OR, NOT logic). Read more about [the supported syntax and
operators](https://msdn.microsoft.com/library/microsoft.visualstudio.shell.interop.ivsbooleansymbolexpressionevaluator.evaluateexpression.aspx).

## Filtering MEF parts via capabilities

Classes exported via MEF can declare the project capabilities under which they apply. See [MEF](mef.md) for more information.

## Defining capabilities via MSBuild

Project capabilities can be declared in several ways, the easiest of which
being to add this MSBuild item to your `.targets` file:

```xml
<ItemGroup>
  <ProjectCapability Include="MyOwnCapability" />
</ItemGroup>
```

## Defining fixed capabilities for a project type

Some capabilities are static/fixed for a given project type. These capabilities should be defined directly on the project type registration.

For example:

```csharp
[assembly: ProjectTypeRegistration(
    projectTypeGuid: MyProjectType.Guid,
    displayName: "#1",
    displayProjectFileExtensions: "#2",
    defaultProjectExtension: "myproj",
    language: "MyLang",
    resourcePackageGuid: MyPackage.PackageGuid,
    Capabilities = "MyProject; AnotherCapability")] // Define capabilities here
```

Capabilities defined via the `ProjectTypeRegistrationAttribute.Capabilities` property are available on all projects loaded for that project type. Multiple values are separated by semicolon (`;`).

Sometimes you'll need capabilities to be defined very early in a project's lifecycle. These fixed capabilities are available from

## Viewing a project's capabilities

To see the capabilities a CPS project defines, add the `DiagnoseCapabilities` project capability to turn on a tree in the VS Solution Explorer that lists all capabilities of the project:

```xml
<ItemGroup>
  <ProjectCapability Include="DiagnoseCapabilities" />
</ItemGroup>
```

![alt text](../Images/diagnose-capabilities-tree.png)

## Common project capabilities and where they are defined

### Existing project capabilities

Some capabilities are defined by default by Microsoft.Common.CurrentVersion.targets.
Be sure to review that file if capabilities are defined that you do not wish
to be defined. You may find that you need to set the `DefineCommonCapabilities`
property to `false` to suppress these capabilities.

Review a [description of all documented project capabilities](project_capabilities.md).

### How do I define my own project capabilities?
    
To define your own project capability, please [Create a new issue][NewIssue]
beginning with the title "Project capability proposal: [capability-name]"
and state the description of the capability. This way we and the community
can help review the proposal for compliance with the above guidelines
and help you define/consume these in the best way for your requirements.
We may even be able to redirect you to an existing project capability
that may suit your needs.

It's very important that project capabilities you define fit this criteria:

- Names a specific technology, type, function, subset, etc. that is actually 
  what you depend on and/or provide. 
  - For example: `WindowsAppContainerPackagedApp`, `ResolveAssemblyReferences`, 
    `ResolveComReferences`, `WindowsXaml`
- Not merely a brand name, a code name, or a broad idea.
  - Bad examples: `Windows8`, `WindowsPhone`, `Immersive`, `Modern`, 
    `CodeSharing`
- More complex than a noun. Perhaps VerbNoun. A capability is something the 
  project can *do* rather than something it *is*. Think of a term that can 
  complete the sentence "I can…"
  - Bad: `Android`. It does not sufficiently describe a capability but merely 
    names a technology. Does this project build on Android or for Android? 
    Does it build the Android platform itself? Does it build androids?
  - Good: `BuildAndroidTarget`
- Have a namespace prefix, or otherwise sufficiently unique so as to keep the 
  odds of it colliding with a project capability someone else (including 
  outside of Microsoft) might come up with.
  - Good: `Microsoft.XYZ.Concurrency`
  - Bad: `Concurrency`
- Avoid acronyms: 
  - Good: `CSharp`
  - Bad: `CS`
- May include a version number, when necessary, but is usually discouraged.

## Dynamic project capabilities

Capablities of a project can be changed without reloading the project.
Read more about [dynamic project capabilities](dynamicCapabilities.md).

 [NewIssue]: https://github.com/Microsoft/VSProjectSystem/issues/new
 
