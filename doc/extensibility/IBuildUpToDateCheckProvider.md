How to add a fast up-to-date check?
===================================

**[Item template:](project_item_templates.md)** Build Up-To-Date Check extension

Applicaple exports of this class are called as part of CPS' implementation of 
`IVsBuildableProjectCfg.StartUpToDateCheck` ([MSDN](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.shell.interop.ivsbuildableprojectcfg.startuptodatecheck.aspx)).

CPS filters the up-to-date check providers into 2 categories: before drain critical tasks, and after.
With each set being called before or after the draining of critical tasks. If you do not depend on
critical tasks it is recommended to set this to `true` so CPS can avoid unnecessary draining of critical
tasks.

Example usage:

```CSharp
[Export(typeof(IBuildUpToDateCheckProvider))]
[ExportMetadata("BeforeDrainCriticalTasks", true)] // Optional, default value is false
[AppliesTo("MyCapability")]
private class RandomBuildUpToDateCheckProvider : IBuildUpToDateCheckProvider
{
    public Task<bool> IsUpToDateAsync(BuildAction buildAction, TextWriter logger, CancellationToken cancellationToken = default(CancellationToken))
    {
        // let fate decide
        return Task.FromResult((new Random()).Next(2) == 0);
    }

    public Task<bool> IsUpToDateCheckEnabledAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // let fate decide
        return Task.FromResult((new Random()).Next(2) == 0);
    }
}
```
