Changes
==========
**Visual Studio 2017**
Download Location: [Visual Studio Project System Extensibility](https://visualstudiogallery.msdn.microsoft.com/43691584-1f0f-46da-adaf-a07c290c1e6e)

Official NuGet packages: http://www.nuget.org

Pre-release NuGet packages: https://myget.org/gallery/vs-devcore

* 15.3.224.43737
  * Targets Visual Studio 2017 Update 3
  * Improved the default debug launcher when starting the script without attaching a debugger (using Ctrl + F5)
    * Fixed unhandled exception
    * Wrapping cscript.exe in cmd.exe, which displays a message and waits for keyboard input
  * Running project system analyzers from command line is now possible (no longer depends on System.Threading.Tasks.Dataflow)
  * Fixed [#234](https://github.com/Microsoft/VSProjectSystem/issues/234) - Xaml Rules compilation stops working when adding WPF Xaml pages 
  * Referencing System.Composition package instead of Microsoft.Composition (which is being deprecated)
* Nuget only 15.0.751 - Updated nuget packages for Visual Studio 2017 Update 2
* Nuget only 15.0.747 - Updated nuget packages for Visual Studio 2017 Update 1
* 15.0.743.997
  * Targets Visual Studio 2017 RTM
  * Project System Extensibility generates a [Windows Script sample project type](samples/WindowsScript) instead of a C#-like project system
    * Simpler, easier to understand structure (no dependency on CSharp targets that have more than 8000 lines of code)
    * Avoids confusion with [roslyn project system](https://github.com/dotnet/roslyn-project-system/)
  * Removed the Preview tag
  * Wizard dialog now has a new field for specifying the language
    * [#152](https://github.com/Microsoft/VSProjectSystem/issues/152) - Put a space in the category name
  * Fixing some inconsistencies when including xaml rule files
    * [#171](https://github.com/Microsoft/VSProjectSystem/issues/171) - Bug in VS2017 Extensibility Preview - Project Type - general.browseobject.xaml wrong build action
    * [#175](https://github.com/Microsoft/VSProjectSystem/issues/175) - VS2017 Extensibility Preview - Custom tools not set
* Nuget only 15.0.688-pre - Updated nuget packages for Visual Studio 2017 RC3
* 15.0.594.65117
  * Targets [Visual Studio 2017 RC](https://www.visualstudio.com/vs/visual-studio-2017-rc/)
  * [.Net Core project](https://github.com/dotnet/roslyn-project-system) is built on top of CPS
    * App Designer support
    * Nuget support
    * Editing the project file is now possible without unloading the project
  * [File globbing support](doc/overview/globbing_behavior.md)
  * [Dynamic Depend Upon Items](doc/extensibility/automatic_DependentUpon_wireup.md)
  * Folder Properties
  * Extending Xaml rules
* 15.0.183.53925
  * Targets Visual Studio 2017 Preview
  * [Breaking changes](doc/overview/breaking_changes_visual_studio_next.md)
  * `Project Tree Modifier extension` item template was replaced by `Project Tree Properties Provider extension`
  * Other fixes and improvements:
    * [#81](https://github.com/Microsoft/VSProjectSystem/issues/81) - Can't override default Xaml rules; build fails when changing project type to Class Library
    * [#82](https://github.com/Microsoft/VSProjectSystem/issues/82) - Analyzers should support project.json
    * [Connect 2293675](https://connect.microsoft.com/VisualStudio/feedback/details/2293675/cant-add-reference-to-project-created-from-an-extensibility-project-type-template-based-project) Reference Manager Dialog displays error in generated project type

**Visual Studio 2015**
Download Location: [Visual Studio Project System Extensibility Preview](https://visualstudiogallery.msdn.microsoft.com/31e107b7-b0ce-4236-8ffa-ed35f03397b8)
* Nuget only 14.1.170-pre - Updated nuget packages for Visual Studio 2015 Update 3
* 14.1.127.50932
  * Targets Visual Studio 2015 Update 2
  * Other fixes and improvements:
    * [#81](https://github.com/Microsoft/VSProjectSystem/issues/81) - Can't override default Xaml rules; build fails when changing project type to Class Library
    * [#82](https://github.com/Microsoft/VSProjectSystem/issues/82) - Analyzers should support project.json
    * [Connect 2293675](https://connect.microsoft.com/VisualStudio/feedback/details/2293675/cant-add-reference-to-project-created-from-an-extensibility-project-type-template-based-project) Reference Manager Dialog displays error in generated project type
* 14.1.80.38181
  * Targets Visual Studio 2015 Update 1
* 14.0.50721.1
  * Targets Visual Studio 2015 RTM
  * New Item Templates
    * [Property Page Value Editor extension](doc/extensibility/property_value_editors.md)
    * [Custom Icons](doc/scenario/provide_custom_icons_for_the_project_or_item_type.md)
    * [Dynamic Enum Values Provider](doc/extensibility/IDynamicEnumValuesProvider.md)
  * Other fixes and improvements
    * Consumes stable versions of [Microsoft.VisualStudio.Threading](https://www.nuget.org/packages/Microsoft.VisualStudio.Threading/) and [Microsoft.VisualStudio.Validation](https://www.nuget.org/packages/Microsoft.VisualStudio.Validation/) Nuget packages
    * `Project Tree Modifier extension` item template - now specifies the `OrderPrecedence` attribute to avoid inconsistent behavior when used in combination with the default project type template
    * `Project Type` template - debugger now functions by default for new project types
    * `Custom Debugger extension` item template - file name now matches the name of the class for the `IDebugLaunchProvider` implementation
    * All item/project templates included in the SDK now use custom icons
* 14.0.50617.1
  * New [Threading Analyzers](https://www.nuget.org/packages/Microsoft.VisualStudio.Threading.Analyzers/)
  * New [Project System Analyzers](https://www.nuget.org/packages/Microsoft.VisualStudio.ProjectSystem.Analyzers)
  * New Item Template: [Command Group Handler extension](doc/extensibility/command_handlers.md)
* 14.0.50430.1
  * Initial release
  
