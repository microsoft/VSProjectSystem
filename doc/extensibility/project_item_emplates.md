Item Templates
====================

The Project System Extensibility SDK offers several item templates.

They can be easily added to your project by using Add New Item. They are located under the Visual C# Items/Extensibility/Project System Node in the Add New Item dialog.

Item Template | Description | More Info
------------ | ------------- | -------------
Project Item Type | Defines new types of items that can be added to your project | [Custom Item Types](extensibility/custom_item_types.md)
Build Up-To-Date Check extension | Provides a hint as to whether the project's outputs are up to date | [IBuildUpToDateCheckProvider](extensibility/IBuildUpToDateCheckProvider.md)
Project Capabilities Provider extension | Export that can declare capabilities on behalf of a project | [IProjectCapabilitiesProvider](extensibility/IProjectCapabilitiesProvider.md)
Project Deploy extension | Export that define the deploy step for a project | [IDeployProvider](extensibility/IDeployProvider.md)
Project Load Veto extension | Export that can prevent loading a project in Visual Studio | IVetoProjectLoad
Project Tree Modifier extension | Export that can tweak the appearance of your project in the Solution Explorer | [IProjectTreeModifier](extensibility/IProjectTreeModifier.md)
Special Files Provider extension | Export than can identify certain well-known files in a project | ISpecialFileProvider
Xaml Rule | Creates an empty xaml rule | [Property Pages](extensibility/property_pages.md)
Custom Debugger extension | Export that defines the debug step for a project | [IDebugLaunchProvider](extensibility/IDebugLaunchProvider.md)

### Future Item Templates
Item Template | Description | More Info
------------ | ------------- | -------------
Command Group Handler extension | Export that defines command handling for a group of commands | [Command Handlers](extensibility/command_handlers.md)
