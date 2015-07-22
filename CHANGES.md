Changes
==========
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
  
