Notification for async project load completion
==============================================

CPS project types load asynchronously, which means if you query them via
COM interfaces to discover items or references, you may get back an empty
collection if your query occurs before post-load population occurs. 

### Option 1

If you just want to be notified when population has completed so you can
create your own snapshot of the project's contents, this code is appropriate:

```csharp
    private class TreeServiceImportHelper
    {
        [Import("Microsoft.VisualStudio.ProjectSystem.PhysicalProjectTreeService")]
        internal IProjectTreeService TreeService { get; set; }
    }

    /// <summary>
    /// Returns a task that completes when the specified project has
    /// populated its IVsHierarchy with items.
    /// </summary>
    /// <param name="unconfiguredProject">The project to wait for population.</param>
    private async Task WaitForItemPopulationAsync(UnconfiguredProject unconfiguredProject)
    {
        var helper = new TreeServiceImportHelper();
        unconfiguredProject.SatisfyImportsOnce(helper);
        await helper.TreeService.PublishAnyNonLoadingTreeAsync();
    }
```

The `unconfiguredProject` argument may be acquired using a technique from
[Finding CPS in a VS project](finding_CPS_in_a_VS_project.md).

Please note: do not call this method followed by `.Wait()` to synchronously
block the UI thread till population has occurred or it will deadlock
when the tree has not already been populated. Please review 
[Threading Rules](3_threading_rules.md) and [VS Scenarios](cookbook.md)
for more on this warning and possible workarounds.

One appropriate way to schedule work to occur after population:

```csharp
    private async Task CaptureItemsSnapshotAsync(UnconfiguredProject unconfiguredProject)
    {
        await WaitForItemPopulationAsync(unconfiguredProject);
        // now acquire snapshot
    }
```

### Option 2

If you're really interested in subscribing to project data (you want the
data as it is now, and you want to receive updates as it changes), then
you should definitely consider the `IProjectSubscriptionService`. This will
give you the initial snapshot of items as soon as they're available and
whenever the data you subscribed to changes, you'll also be notified with
snapshots for both before and after, as well as a semantic diff describing
what changed.

See [Subscribe to project data](subscribe_to_project_data.md)
