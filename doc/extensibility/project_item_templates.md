Item Templates
====================

The Project System Extensibility SDK offers several item templates.

They can be easily added to your project by using Add New Item. They are located under the Visual C# Items/Extensibility/Project System Node in the Add New Item dialog.

Item Template | Description | More Info
------------ | ------------- | -------------
Project Item Type | Defines new types of items that can be added to your project | [Custom Item Types](custom_item_types.md), [Tutorial](../overview/contentitem_types.md)
Build Up-To-Date Check extension | Provides a hint as to whether the project's outputs are up to date | [IBuildUpToDateCheckProvider](IBuildUpToDateCheckProvider.md)
Project Capabilities Provider extension | Export that can declare capabilities on behalf of a project | [IProjectCapabilitiesProvider](IProjectCapabilitiesProvider.md)
Project Deploy extension | Export that define the deploy step for a project | [IDeployProvider](IDeployProvider.md)
Project Load Veto extension | Export that can prevent loading a project in Visual Studio | [IVetoProjectLoad](IVetoProjectLoad.md)
Project Tree Modifier extension | Export that can tweak the appearance of your project in the Solution Explorer | [IProjectTreeModifier](IProjectTreeModifier.md)
Special Files Provider extension | Export than can identify certain well-known files in a project | [ISpecialFileProvider](ISpecialFileProvider.md)
Xaml Rule | Creates an empty xaml rule | [Property Pages](property_pages.md)
Custom Debugger extension | Export that defines the debug step for a project | [IDebugLaunchProvider](IDebugLaunchProvider.md)
Command Group Handler extension | Export that defines command handling for a group of commands | [Command Handlers](command_handlers.md)
Property Page Value Editor extension | Export that provides a custom property value editor. | [Property value editors](property_value_editors.md)
Custom Icons | Adds a set of custom images that can be consumed by project type using an IProjectTreeModifier export. | [Provide Custom Icons](../scenario/provide_custom_icons_for_the_project_or_item_type.md) [IProjectTreeModifier](IProjectTreeModifier.md)
Dynamic Enum Values Provider | Export that provides a dynamic list of enum values | [IDynamicEnumValuesProvider](IDynamicEnumValuesProvider.md)

### Future Item Templates
Item Template | Description | More Info
------------ | ------------- | -------------

