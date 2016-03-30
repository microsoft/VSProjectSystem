Dynamic Capabilities - Visual Studio Next
====================

Project capabilities are the recommended way to determine the type, platform,
and features of a project. In the modern world, features of a project can change over the time.
For example, when a NuGet package is added.  In the Visual Studio Next,
the CPS has a built-in infrastructure to support capabilities of a project to be changed without
reloading the project.

This infrastruture will eventually allow the design time experience of
a project to be adjusted dynamically based on features/NuGet packages being used in the project.
For example, WPF features can be turned on, only because the project references WPF related packages,
instead of depending on which template was used when the project was created.
 

Capabilities are now coming from dataflows
---------------------------------------------------
In both unconfigured project and configured project scopes, capabilities are no longer a fixed set of strings. 
They are now exposed as a dataflow source of `IProjectCapabilitiesSnapshot`.

For code which only cares about the current status of the project, it can get the current value,
like this:

```csharp
    project.Capabilities.Current.Value.IsProjectCapabilityPresent("SomeCapability");
```

Or use a built-in extension method in CPS, which is recommended over the pervious method:

```csharp
    project.Capabilities.Contains("SomeCapability");
```

To detect capability changes, you need to chain to the data source:

```csharp
    var myCapabilitiesReceiver = new ActionBlock<IProjectVersionedValue<IProjectCapabilitiesSnapshot>>(...);
    project.Capabilities.SourceBlock.LinkTo(myCapabilitiesReceiver);
```

More often, CPS extensions are MEF components, and they import other CPS components filtered by project
capabilities. The following sample still works as before:

```csharp
        [ImportMany]
        private OrderPrecedenceImportCollection<IDeployProvider> DeployProviders { get; set; }
        
        [ImportingConstructor]
        public MyComponent(ConfiguredProject configuredProject)
        {
            this.DeployProviders = new OrderPrecedenceImportCollection<IDeployProvider>(
                projectCapabilityCheckProvider: configuredProject);
        }       
```

The `DeployProviders` collection will contain a set of `IDeployProvider` filtered by the current capabilities
of the configuredProject. The content of the collection can change over the time. 

How to prevent seeing capability changes in the middle of an execution
---------------------------------------------------
When changes are made to the project, just like other dataflows inside CPS, capabilites are being recalculated
in the background, and published when it is ready. Without proper protection, the content of a collection
like the DeployProviders in the earlier sample can change at any moment. It would be hard to write stable
code in this environment.

To prevent this, you can create a `ProjectCapabiltiesContext`, and wrap all the logic
inside it. Implemented as a part of `ExecutionContext`, the context will apply to methods called inside, and
also async tasks started inside it. 

```csharp
    using (ProjectCapabiltiesContext.CreateContext(currentProject))
    {
        // Inside a ProjectCapabiltiesContext, project.Capabilities.Contains("SomeCapability")
        // and DeployProviders will always return the same result.
    }
```

Block unsupported capabilities changes
---------------------------------------------------
A project system or its extensions may not be able to handle some capabilities changes,
and may want to trigger the project to be reloaded in that case.
That can be done by registering requirements through `IProjectCapabilitiesRequirementsService`.
If new set of capabilities violates any of those rules, it will force to reload the project automatically.

An easy way to register requirements is through creating a `ProjectCapabilitiesContext`.  The following
sample will register capabilities requirements to ensure the result of `DeployProviders` stays unchanged
for the project in the current session:

```csharp
    List<IDeployProvider> effectiveProviders;
    using (ProjectCapabiltiesContext.CreateContext(
        currentProject,
        DependencyManagementType.InvalidateProjectState))
    {
        effectiveProviders = this.DeployProviders.ToList();
    }
```

Automatically load components when a new capability is added
---------------------------------------------------
Project systems or extensions can add `IProjectDynamicLoadComponent` through MEF. 
When capabilities requirements are satisfied, CPS will load the component, 
and unload it when the condition is no longer satisified.

```csharp
        [Export(ExportContractNames.Scopes.UnconfiguredProject, typeof(IProjectDynamicLoadComponent))]
        [AppliesTo("MyExtensionCapability")]
        internal class MyExtensionCapabilityFeature : IProjectDynamicLoadComponent
        {
            public async Task LoadAsync()
            {
                // Initialize the feature
            }

            public async Task UnloadAsync()
            {
                // Unload the feature
            }
       }
 ```
       
