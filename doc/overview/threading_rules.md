3 Threading Rules
=================

The 3 Threading Rules can now be found at [Microsoft/vs-threading/doc/threading_rules.md](Microsoft/vs-threading/doc/threading_rules.md)


CPS Threading
=================================

### CPS `JoinableTaskFactory`

When calling into CPS specific API's (anything underneath the `Microsoft.VisualStudio.ProjectSystem` namespace) you __must__
use the `IProjectThreadingService.JoinableTaskFactory` instead of the Shell `ThreadHelper.JoinableTaskFactory`. This is
because the CPS JTF has additional knowledge to mitigate deadlocks from project lock and UI thread contentions.
[Obtaining the IProjectThreadingService.](../automation/obtainging_the_IThreadHandling_service.md)


### `IVs*` Interfaces From `Microsoft.VisualStudio.ProjectSystem`

Unlike most `IVs*` interfaces from `Microsoft.VisualStudio.Shell` and other VS namespaces, `IVs` interfaces from CPS have
__no__ thread affinity. These interfaces can be called from any thread by CPS. The `IVs` naming is to clarify that these
interfaces are specific to CPS in VS and not CPS Core.