Obtaining the MSBuild.Project from CPS
======================================

1. Acquire the [`IProjectLockService`](obtaining_the_IProjectLockService.md)
2. [Obtain a `ConfiguredProject`](finding_CPS_in_a_VS_project.md) for which 
   you want to get the MSBuild project object.
3. Acquire a read, upgradeable read or write lock, as appropriate, and 
   use the MSBuild Project object exclusively within the lock:

```csharp
await projectLockService.WriteLockAsync(
    async access =>
    {
        Project project = await access.GetProjectAsync(configuredProject);

        // Use the msbuild project, respecting the type of lock acquired.

        // If you're going to change the project in any way, 
        // check it out from SCC first:
        await access.CheckoutAsync(configuredProject.UnconfiguredProject.FullPath);
    },
    cancellationToken);
```

Note that it's important that you use `await`. Do not use `Task.Result` or
`Task.Wait()` on these async methods or your code will malfunction and/or hang.
If you must do this within a synchronous method, see [threading 
rule #2](https://github.com/microsoft/vs-threading/blob/main/docfx/docs/threading_rules.md#-rule-2-use-jtfrun).

**Please observe CPS [project locking rules](../overview/project_lock.md) by not
retaining any references to MSBuild objects beyond the scope of the lock and
only using these objects while not on the UI thread.  Violating this exposes
your code and other project-related code to the risk of multithread-related
IDE crashes, even if you're just reading the project.**
