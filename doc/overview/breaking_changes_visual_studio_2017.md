# Project System Extensibility Breaking Changes for Visual Studio 2017

We have made several breaking changes for the next version of Project System Extensibility. Our goal with these changes is to provide a consistent assembly name over releases, simplified namespaces, and new features. Below is a list of these changes.  

## Assembly Rename and Consolidation

We have renamed all of our assemblies to the following:

Old Name | New Name
--- | ---
Microsoft.VisualStudio.ProjectSystem.v14Only | Microsoft.VisualStudio.ProjectSystem
Microsoft.VisualStudio.ProjectSystem.VS.v14Only | Microsoft.VisualStudio.ProjectSystem.VS

Microsoft.VisualStudio.ProjectSystem.Utilities.14.0 has been removed, and its contents merged into the other 2 assemblies.

## Namespace Changes

Namespaces have been heavily refactored.

| New Namespaces |
| --- |
| Microsoft.VisualStudio.ProjectSystem |
| Microsoft.VisualStudio.ProjectSystem.Build |
| Microsoft.VisualStudio.ProjectSystem.Debug |
| Microsoft.VisualStudio.ProjectSystem.Properties |
| Microsoft.VisualStudio.ProjectSystem.References |
| Microsoft.VisualStudio.ProjectSystem.VS |
| Microsoft.VisualStudio.ProjectSystem.VS.Build |
| Microsoft.VisualStudio.ProjectSystem.VS.Debug |
| Microsoft.VisualStudio.ProjectSystem.VS.Properties |

## Interface Merging
In VS2015 update 1 we added some '2' versions of various interfaces. 
These interfaces have been merged into the original version of the interface. 

Interface Source | Interface Target
--- | ---
`IProjectService2` | `IProjectService`
`IProjectTreeFactory2` | `IProjectTreeFactory`
`IVsLoggerProvider2` | `IVsLoggerProvider`
`IBuildProject2` | `IBuildProject`
`IProjectItemProvider2`* | `IProjectItemProvider`

\* the addition methods from `IProjectItemProvider2` where added onto `ProjectItemProviderBase`

## Adds, Removals, and Renames

### Added

New | Description
--- | ---
`IProjectTreePropertiesProvider` | New extension point for modifying `IProjectTree`
`NamedIdentity` | Used for naming and identifiying `IProjectValueDataSource`
`ProjectTreeFlags` | Replacement for `IProjectTree.Capabilities`
`IProjectDynamicLoadComponent` | New extension point for components that support being dynamically loaded/unloaded
`IProjectCapabilitiesScope` | Scopes the current capabilities to support dynamic capabilities

### Removals

Removed | Description
--- | ---
`UnconfiguredProjectAutoLoadAttribute` | Replaced by `ProjectAutoLoadAttribute`
`IProjectTreeModifier` | Replaced by `IProjectTreePropertiesProvider`
`WeakKeyDictionary` | -
`IProjectCapabilityCheckProvider` | Replaced by `IProjectCapabilitiesScope`
`ProjectCapabilitiesKeyPropertiesProvider` | -
`IProjectValueDataSource<T>.DataSourceKey` | Appears on `IProjectValueDataSource`
`IProjectValueDataSource<T>.DataSourceVersion` | Appears on `IProjectValueDataSource`
`UnconfiguredProject.GetCapabilitiesAsync` | replaced by `UnconfiguredProject.Capabilities`
`ConfiguredProject.GetCapabilitiesAsync` | replaced by `ConfiguredProject.Capabilities`

### Renames

Old Name | New Name
--- | ---
`ProjectService` | `IProjectService`
`UnconfiguredProjectAutoLoad2Attribute` | `ProjectAutoLoadAttribute`
`OrderPrecedenceAttribute` | `OrderAttribute`
`IThreadHandling` | `IProjectThreadingService`
`IThreadHandling.AsyncPump` | `IProjectThreadingService.JoinableTaskFactory`
`IProjectTree.Capabilities` | `IProjectTree.Flags`
`IProjectReloader.ReloadIfCapabilitiesChangedAsync` | `IProjectReloader.ReloadIfNecessaryAsync`


## Behavioral Changes

* Dynamic capability support added. This makes capabilities a part of the data flow. More information can be found [here](dynamicCapabilities.md).
* The contract for `ProjectAutoLoadAttribute` (previously `UnconfiguredProjectAutoLoadAttribute`) has changed from `void Initialize()` to `Task Initialize()`
* `IProjectThreadingService.SwitchToUIThread` is now an extension method
* `IProjectTree.Flags` is a new struct `ProjectTreeFlags` which is a hybrid of `enum` and `ImmutableHashSet<string>`. Most commonly used flags are found in `enum ProjectTreeFlags.Common`.
* `ExportCommandGroupAttribute.Group` is now a `Guid` (formerly `string`). The constructor accepts either a string or guid.
* Implementation of `IVsProject.IsDocumentInProject` changed to return not found for items of `DP2_NonMember` and below. Previously it would return true for excluded items underneath the project directory.
  * You can use `IVsHierarchy.ParseCanonicalName` to check if an item is under the project directory, but may not be included.
* `IProjectSubscriptionService.XXXBlock` marked obsolete. Use `IProjectSubscriptionService.XXXSource.SourceBlock` instead.
* `IProjectValueDataSource.DataSourceKey` changed from `Guid` to a new class `NamedIdentity`. This allows for naming the data source with a string.
* `StaticGlobalPropertiesProviderBase` constructor now takes only an `IProjectCommonServices`.
* `UnconfiguredProject.Capabilities` and `ConfiguredProject.Capabilities` type changed from `IImmutableSet<string>` to `IProjectCapabilitiesScope`
