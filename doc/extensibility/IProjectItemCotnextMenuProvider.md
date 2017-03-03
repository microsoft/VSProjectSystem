# IProjectContextMenuProvider

Extension point for suppling the context menu group `Guid` and the menu Id `int` to display
to the user. This interface does not let you _implement_ new context menus to show to the
user, but only lets you select what menu to show based on the `IProjectTree` selected.
See [Extending Menus and Commands](https://msdn.microsoft.com/en-us/library/bb165937.aspx)
for information on how to implement context menus.

## Extension Point

* Contract: `[Export(typeof(IProjectItemContextMenuProvider))]`
* Metadata: No special metadata. Uses `[Order]` and `[AppliesTo]`

## Interface

``` CSharp
/// <summary>
/// An extension component to provide context menu to be shown in the solution explorer for a specific project item.
/// </summary>
[ProjectSystemContract(ProjectSystemContractScope.UnconfiguredProject, ProjectSystemContractProvider.Extension)]
public interface IProjectItemContextMenuProvider
{
    /// <summary>
    /// Gets the context menu for a project item.
    /// </summary>
    /// <param name="projectItem">The project item</param>
    /// <param name="menuCommandGuid">The menu command guid to retrieve the menu</param>
    /// <param name="menuCommandId">The menu command id to retrieve the menu</param>
    /// <returns>True, if the provider knows the context menu to be used.</returns>
    bool TryGetContextMenu(IProjectTree projectItem, out Guid menuCommandGuid, out int menuCommandId);

    /// <summary>
    /// Gets the context menu, when different type of project items are selected.
    /// This function is only called when TryToGetContextMenu returns different menu for selected items.
    /// </summary>
    /// <param name="projectItems">Multiple project items</param>
    /// <param name="menuCommandGuid">The menu command guid to retrieve the menu</param>
    /// <param name="menuCommandId">The menu command id to retrieve the menu</param>
    /// <returns>True, if the provider knows the context menu to be used.</returns>
    bool TryGetMixedItemsContextMenu(IEnumerable<IProjectTree> projectItems, out Guid menuCommandGuid, out int menuCommandId);
}
```

## Behavior

First `TryGetContextMenu` is called on each provider for every node selected by the user.
If all returned `menuCommandGuid` and `menuCommandId` are the same, those results are used.
If there is a conflicting results, `TryGetMixedItemsContextMenu` is called on each provider
to get a merged result. The first successful merged result returned wins. The returned
values are passed to [IVsUIShell.ShowContextMenu](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsuishell.showcontextmenu.aspx).

CPS implements a default provider that covers common items. Implement your own if you require
special behavior.

## Example

``` CSharp
[Export(typeof(IProjectItemContextMenuProvider))]
[AppliesTo(ProjectCapabilities.MyCapability)]
[Order(MyOrder)] // higher values are processed first, choose yours accordingly
internal class DefaultProjectItemContextMenuProvider : IProjectItemContextMenuProvider
{
    public bool TryGetContextMenu(IProjectTree projectItem, out Guid menuCommandGuid, out int menuCommandId)
    {
        menuCommandGuid = MyCommandMenu;
        if (projectItem.Flags.Contains("MyFlag"))
        {
            menuCommandId = MyCommandId;
            return true;
        }

        return false;
    }

    public bool TryGetMixedItemsContextMenu(IEnumerable<IProjectTree> projectItems, out Guid menuCommandGuid, out int menuCommandId)
    {
       return false; // we let others display a mixed item menu
    }
}
```