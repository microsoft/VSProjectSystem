Project Capabilities
====================

Project capabilities are the recommended way to determine the type, platform,
and features of a project. Simply checking the language or the project
file extension does not help if you want to check for WPF vs. WinForms
vs. Windows 8 XAML, for example. There are a great many different aspects
to a project that may be present regardless of language. Do you want code
that runs against any Windows 8 targeting project regardless of language?
Do you want to target just Javascript Win8 projects but not LightSwitch
JS projects? Project capability checks are the answer.

The presence of some capability can be detected on a given project with
code such as:

    using Microsoft.VisualStudio.Shell; // imports the extension method

    IVsHierarchy hierarchy;
    bool match = hierarchy.IsCapabilityMatch("SomeCapability");

Where [`IVsHierarchy.IsCapabilityMatch`][l01] is an extension method.
[l01]:http://msdn.microsoft.com/en-us/library/vstudio/hh443055.aspx

Project capability expressions can also be passed to the `IsCapabilityMatch`
method in order to test for combinations of capabilities (including
AND, OR, NOT logic). Read more about [the supported syntax and
operators](http://msdn.microsoft.com/en-us/library/vstudio/microsoft.visualstudio.shell.interop.ivsbooleansymbolexpressionevaluator.evaluateexpression.aspx).

Common project capabilities and where they are defined
------------------------------------------------------

### Existing project capabilities

Some capabilities are defined by default by Microsoft.Common.CurrentVersion.targets.
Be sure to review that file if capabilities are defined that you do not wish
to be defined. You may find that you need to set the `DefineCommonCapabilities`
property to `false` to suppress these capabilities.

Review a [description of all documented project capabilities](project_capabilities.md).

### How do I define my own project capabilities?

Project capabilities can be defined in several ways, the easiest of which
being to add this MSBuild item to your .targets file:

    <ProjectCapability Include="MyOwnCapability" />

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
  - Bad: `VB`
- May include a version number, when necessary, but is usually discouraged.
    
Please email [vscpsfte@microsoft.com][mail] before defining your own new project
capabilities so we can help review for compliance with the above guidelines
and help you define/consume these in the best way for your requirements.
[mail]:mailto:vscpsfte@microsoft.com

Note that the link is not publicly accessible

