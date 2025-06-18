﻿# The 3 Threading Rules

The 3 Threading Rules can now be found in the [microsoft/vs-threading repo](https://github.com/microsoft/vs-threading/blob/main/docfx/docs/threading_rules.md).

# CPS Threading

## CPS `JoinableTaskFactory`

When calling into CPS specific API's (anything underneath the `Microsoft.VisualStudio.ProjectSystem` namespace) you __must__
use the `IProjectThreadingService.JoinableTaskFactory` instead of the Shell `ThreadHelper.JoinableTaskFactory`. This is
because the CPS JTF has additional knowledge to mitigate deadlocks from project lock and UI thread contentions.
[Obtaining the IProjectThreadingService.](../automation/obtaining_the_IThreadHandling_service.md)


## `IVs*` Interfaces From `Microsoft.VisualStudio.ProjectSystem`

Unlike most `IVs*` interfaces from `Microsoft.VisualStudio.Shell` and other VS namespaces, `IVs` interfaces from CPS have
__no__ thread affinity. These interfaces can be called from any thread by CPS. The `IVs` naming is to clarify that these
interfaces are specific to CPS in VS and not CPS Core.