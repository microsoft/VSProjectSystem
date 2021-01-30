`IProjectTreePropertiesProvider` - Visual Studio 2017
======================
**Visual Studio 2015:** use [IProjectTreeModifier, IProjectTreeModifier2](IProjectTreePropertiesProvider.md)

**[Item template:](project_item_templates.md)** Project Tree Properties Provider extension

**Scope:** UnconfiguredProject

The `IProjectTreePropertiesProvider` interface can be exported into the 
`UnconfiguredProject` scope to customize some properties of a project tree node. 
`IProjectTreePropertiesProvider.CalculatePropertyValues` is called back
for every visible and non-visible node in the tree.

This allows you to, for example, set the icon on 
your project's root node to be consistent with your branding by overriding the values set by lower priority providers.
You can also set the icons on specific source items that are may be unique to your project type.

```CSharp
    /// <summary>
    /// Updates nodes in the project tree by overriding property values calcuated so far by lower priority providers.
    /// </summary>
    [Export(typeof(IProjectTreePropertiesProvider))]
    [AppliesTo(MyUnconfiguredProject.UniqueCapability)]
    // TODO: For new project types, consider removing this Order attribute. If customizing an existing project type, you may need a high enough value to ensure your implementation overrides the base project's behaviour.
    [Order(1000)]
    internal class ProjectTreePropertiesProvider1 : IProjectTreePropertiesProvider
    {
        /// <summary>
        /// Calculates new property values for each node in the project tree.
        /// </summary>
        /// <param name="propertyContext">Context information that can be used for the calculation.</param>
        /// <param name="propertyValues">Values calculated so far for the current node by lower priority tree properties providers.</param>
        public void CalculatePropertyValues(
            IProjectTreeCustomizablePropertyContext propertyContext,
            IProjectTreeCustomizablePropertyValues propertyValues)
        {
            // Only set the icon for the root project node.  We could choose to set different icons for nodes based
            // on various criteria, not just Capabilities, if we wished.
            if (propertyValues.Flags.Contains(ProjectTreeFlags.Common.ProjectRoot))
            {
                // TODO: Provide a moniker that represents the desired icon (you can use the "Custom Icons" item template to add a .imagemanifest to the project)
                propertyValues.Icon = KnownMonikers.JSProjectNode.ToProjectSystemType();
            }
        }
    }
```

Do NOT use this to modify some structural flags of the tree. For example, marking a file item
to be a folder will cause malfunctions in the project system. 

You can create an `IProjectTreePropertiesProvider` export using the "Project Tree
Modifier extension" item template included in the CPS SDK.

