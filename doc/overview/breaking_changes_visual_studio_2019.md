# Project System Extensibility Breaking Changes for Visual Studio 2019

We have made several breaking changes for the next version of Project System Extensibility. Our goal with these changes is to provide a consistent assembly name over releases, simplified namespaces, and new features. Below is a list of these changes.

For breaking changes made in previous releases:
  * [Breaking changes in Visual Studio 2017](breaking_changes_visual_studio_2017.md)
  * [Breaking changes in Visual Studio 2015](VS2013_vs_VS2015.md)

## Changed the type of the `Services` object

The type `Services` object of `IProjectService`/`UnconfiguredProject`/`ConfiguredProject` has been changed from interface to abstract class, so new services can be added without introducing breaking changes in the future.

While there is no code change needed to adopt this breaking change, a **rebuild of the component is required** with the Visual Studio 2019 version of CPS reference assembly make the component to work in Visual Studio 2019.

## Some snapshot interfaces no longer inherit from `IProjectVersionedValue`

The following interfaces no longer inherit from `IProjectVersionedValue`:

| Interface |
| --- |
| IProjectSnapshot |
| IProjectRuleSnapshot |
| IProjectSharedFoldersSnapshot |
| IProjectImportTreeSnapshot |

The project version is already carried in the dataflow. This is a breaking change.

In most cases, we added a new `Value` property to the interface, which originally used to inherit the property from `IProjectVersionedValue<T>`.

Any code using the version number in those snapshots should use the version number in the dataflow directly.

`IProjectSnapshot.Value` has been marked as obsolete, please use `IProjectSnapshot.ProjectInstance`.

## `IBuildLoggerProvider` has been marked as obsolete, and is no longer called

Code should be changed to use `IBuildLoggerProviderAsync` instead

## `IVsLoggerEventProcessor` is removed

It was marked as obsolete in the previous release, and not called by the product.

Mitigation: use `IVsLoggerEventProcessor2` instead

## Property `ActiveConfigurationChangedEventArgs.ActiveConfiguredProjectProviderDataSourceVersion` is removed

It was marked as obsolete in previews version.

## Enum value `ProjectFaultSeverity.Crippling` has been removed.

It was renamed to `LimitedFunctionality` in Visual Studio 2017 updates, and the old enum value has been marked as obsolete for a while.

## `LazyFill` value has been removed from `IProjectTreeSnapshot` and `TreeUpdateResult`

This value was not used and maintained since Dev 14.

## `IProjectTree.PropertySheet` is marked as obsolete and no longer saved in the tree

We don't expect it to be used.

## `IProjectLockService` introduces a new set of delegate based API to replace the current using style API to avoid potential deadlocks

The old using style API has not been removed or marked as obsolete at this point of time, so no action is required.

But we will eventually mark them as obsolete later, so plan some time to move to the new API after Preview 1.

Before:
```csharp
using (var access = await this.ProjectLockService.ReadLockAsync(cancellationToken))
{
    var project = await access.GetProjectAsync(this.ConfiguredProject);
    // Use project...
}
```
	
After:
```csharp
await this.ProjectLockService.ReadLockAsync(
    async access =>
    {
        Project project = await access.GetProjectAsync(this.ConfiguredProject);
        // Use project...
    },
    cancellationToken);
```

## New Slim DataFlow blocks

To reduce the memory usage of dataflow blocks, CPS introduces a new custom set of slim dataflow blocks corresponding to `TransformBlock`/`ActionBlock`/`BroadcastBlocks`.
While this is not a breaking change, we recommend CPS extensions to start using the new blocks.

The new blocks allow CPS dataflow to skip middle versions easier, but may introduce some behavior changes.

Before:
```csharp
var actionBlock = new ActionBlock<IProjectVersionedValue<IProjectSubscriptionUpdate>>(
    this.OnSubscriptionUpdate,
    new ExecutionDataflowBlockOptions { NameFormat = "My Block {1}"});
```

After:
```csharp
var actionBlock = DataflowBlockSlim.CreateActionBlock<IProjectVersionedValue<IProjectSubscriptionUpdate>>(
    this.OnSubscriptionUpdate,
    nameFormat: "My Block {1}");
```

## `IProjectSubscriptionServices` behavior changes

When you chain the dataflow with `initialDataAsNew` `false`, the `ProjectChangeDescription.Before` is now the same as `ProjectChangeDescription.After`.

Previously, the `Before` property will provide an empty snapshot.

If you receive data from those dataflow blocks directly, you will always get the latest snapshots with an empty change delta (`Change.Before` is always the same as `Change.After`).

## `IProjectTree.IsLinked` is now preserved as a flag in the project tree flags

This doesn't affect code that access the property, but only tree providers that create linked item node.

## We are considering making `IProjectTree.BrowseObjectProperties` to be computed lazily post preview 1.