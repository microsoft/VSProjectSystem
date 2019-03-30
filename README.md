# Visual Studio Project System Extensibility Documentation

## What is a project system?
A project system sits between a project file on disk (for example, .csproj and .vbproj) and various Visual Studio features including, but not limited to, Solution Explorer, designers, the debugger, language services, build and deployment. Almost all interaction that occurs with files contained in a project file, happens through the project system.

There are three reasons to extend a project system in Visual Studio:
1. Support a new project file format.
1. Integrate existing file format with a new language service.
1. Customize behavior of an existing project system.

## Project system extensibility
The traditional way to a build or customize a project system  is to implement a set of [Project System COM interfaces](https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-a-basic-project-system-part-1). Most project systems don't start from scratch. Instead, they leverage the [MPFProj project system example](https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/using-the-managed-package-framework-to-implement-a-project-type-csharp?view=vs-2017) as a starting point.

The Visual Studio Project System (VSPS) described in this repository provides default implementation for a subset of project system COM interfaces. This simplifies building and maintaining a project system, but comes with the cost of reduced functionality.

## How to select a project system platform?
|Scenario|Recommended Project System Platform
|---|---
|Customize C#/VB/F# Desktop project system| COM-based [project flavoring](https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/project-types?view=vs-2017).
|Customize  C#/VB/F# .NET Core project system| Not fully supported at this time. VSPS is your best bet.
|Add new a project type or a language| Prototype with VSPS, but be ready to fall back to MPFProj for complex scenarios that VSPS doesn't support yet.

## Compare MPFProj and Visual Studio Project System
|Criteria|[MPFProj](https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/using-the-managed-package-framework-to-implement-a-project-type-csharp?view=vs-2017)| Visual Studio Project System (this repo)
|---|---|---
|Maturity|Release|Preview|
|Breaking changes in major updates|None|[Expected](changes)|
|Flexibility|Full control over project system behavior|Some project system interfaces are not implemented
|Extensibility|C++ and COM-based|C# with managed interfaces and MEF
|Threading model|Single threaded and bound to the UI thread|Multi-threaded, scalable, and responsive
|Scalability|Memory-optimized|Uses extra memory to support multi-threading
|Methodology|Requires implementing all project system interfaces|Provides default implementation and allows for customizations
|Ramp up time|High|Low
|Used by project systems|Desktop C#/VB/F#|[.NET Core C#/VB/F#](https://github.com/dotnet/project-system) and C++
|Minimum Supported Version| Visual Studio 2008 | Visual Studio 2015
 
## Visual Studio Project System
Visual Studio can be extended in many ways, including adding new types of projects and augmenting
existing ones. This repository contains the [documentation and best practices][1] for
creating extensions that add new project types to Visual Studio. We welcome community input if you
wish to contribute new topics or find any issues.

To get started, read the [introduction][intro] while you're waiting for the [pre-requisites][prereq] to install.

Please file any product bugs you find on [Developer Community](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-report-a-problem-with-visual-studio). 
You may file doc bugs [here][docbugs].

[![Join the chat at https://gitter.im/Microsoft/extendvs](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Microsoft/extendvs?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

For the latest features take a look at [Changes][changes].

For further information about extending Visual Studio in other ways please check out
[VisualStudio.com/integrate][3].

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

 [1]: doc/Index.md
 [2]: http://aka.ms/vsprojectsystemextensibilityvsix
 [3]: https://www.visualstudio.com/integrate/explore/explore-vside-vsi?wt.mc_id=o~display~github~vsproject
 [4]: https://www.visualstudio.com/en-us/downloads/visual-studio-2015-downloads-vs.aspx
 [VSSDK]: http://go.microsoft.com/?linkid=9877247
 [prereq]: doc/overview/prereqs.md
 [intro]: doc/overview/intro.md
 [changes]: CHANGES.md
 [docbugs]: https://github.com/Microsoft/VSProjectSystem/issues
 
